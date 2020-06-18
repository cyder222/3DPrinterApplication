using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Tao.OpenGl;
using Tao.FreeGlut;
namespace Jusin.ThreeDLib
{
        #region struct SelectionData

        /// <summary>
        /// セレクションモードでヒットしたオブジェクトのデータを表す。
        /// </summary>
        public class SelectionData
        {
            /// <summary>
            /// 識別番号の階層の深さ
            /// </summary>
            public int nameStackDepth;

            /// <summary>
            /// ヒットしたオブジェクトの深度の最大値
            /// </summary>
            public double zMax;
            /// <summary>
            /// ヒットしたオブジェクトの深度の最小値
            /// </summary>
            public double zMin;

            /// <summary>
            /// ヒットしたオブジェクトの識別番号
            /// </summary>
            public int[] names;


            /// <summary>
            /// ヒットしたオブジェクト
            /// </summary>
            public ISelectable item;


            #region ParseSelectionBuffer

            /// <summary>
            /// uint[]配列のセレクション・バッファを解析して、
            /// SelectionDataのリストに変換する。
            /// </summary>
            /// <param name="selectionBuffer">セレクションモードで得られたセレクション・バッファ</param>
            /// <returns>ヒットしたデータのリスト</returns>
            public static List<SelectionData> ParseSelectionBuffer(uint[] selectionBuffer)
            {
                List<SelectionData> dataList = new List<SelectionData>();
                int i = 0;

                try
                {
                    for (i = 0; i < selectionBuffer.Length; )
                    {
                        if (selectionBuffer[i] == 0) break;

                        SelectionData data = new SelectionData();

                        data.nameStackDepth = (int)selectionBuffer[i++];

                        data.zMin = (double)selectionBuffer[i++] / (double)uint.MaxValue;
                        data.zMax = (double)selectionBuffer[i++] / (double)uint.MaxValue;

                        data.names = new int[data.nameStackDepth];
                        for (int j = 0; j < data.nameStackDepth; ++j)
                        {
                            data.names[j] = (int)selectionBuffer[i++];
                        }

                        dataList.Add(data);
                    }
                }
                catch (IndexOutOfRangeException ex)　//selectionBufferの長さが足りないと、IndexOutOfRangeExceptionが発生してここに来る。
                {
                    if (i >= selectionBuffer.Length) // つまり、selectionBufferの長さが足りない
                    {
                        //この場合は、うまく取得できたデータだけを返して、長さが足りない分は無視。
                    }
                    else
                    {   //それ以外は想定外のエラーなので、とりあえずエラーを投げておく。
                        throw new Exception("ParseSelectionBuffer failed.", ex);
                    }
                }
                catch (Exception ex)
                {   //IndexOutOfRangeException以外は想定外のエラーなので、とりあえずエラーを投げておく。
                    throw new Exception("ParseSelectionBuffer failed.", ex);
                }
                return dataList;
            }

            #endregion ParseSelectionBuffer


        }

        #endregion struct SelectionData


        //#######################################
        //#######################################


        #region class TransformMatrixes

        /// <summary>
        /// <para>ビューポート、プロジェクション行列、モデルビュー行列のセット</para>
        /// <para>OpenGLで使用する変換行列のセット。</para>
        /// </summary>
        public class TransformMatrixes : ICloneable
        {

            #region プロパティ


            #region int[] Viewport

            /// <summary>
            /// ビューポート
            /// </summary>
            private int[] viewport = new int[] { 0, 0, 100, 100 };

            /// <summary>
            /// ビューポートを取得・設定する。
            /// </summary>
            public int[] Viewport
            {
                get { return (int[])this.viewport.Clone(); }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    else if (value.Length != 4) throw new ArgumentException("The length of value is not 4.");
                    value.CopyTo(this.viewport, 0);
                }
            }

            #endregion


            #region double[] Projection

            /// <summary>
            /// プロジェクション行列
            /// </summary>
            /// 
            private double[] projection = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            /// <summary>
            /// プロジェクション行列を取得・設定する。
            /// </summary>
            public double[] Projection
            {
                get { return (double[])this.projection.Clone(); }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    else if (value.Length != 16) throw new ArgumentException("The length of value is not 16.");
                    value.CopyTo(this.projection, 0);
                }
            }

            #endregion


            #region double[] Modelview

            /// <summary>
            /// モデルビュー行列
            /// </summary>
            private double[] modelview = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            /// <summary>
            /// モデルビュー行列を取得・設定する。
            /// </summary>
            public double[] Modelview
            {
                get { return (double[])this.modelview.Clone(); }
                set
                {
                    if (value == null) throw new ArgumentNullException("value");
                    else if (value.Length != 16) throw new ArgumentException("The length of value is not 16.");
                    value.CopyTo(this.modelview, 0);
                }
            }

            #endregion


            #endregion プロパティ


            /// <summary>
            /// TransformMatrixesクラスのインスタンスを初期化する。
            /// </summary>
            /// <param name="viewport">ビューポート</param>
            /// <param name="projecton">プロジェクション行列</param>
            /// <param name="modelview">モデルビュー行列</param>
            public TransformMatrixes(int[] viewport, double[] projecton, double[] modelview)
            {
                this.Viewport = viewport;
                this.Projection = projecton;
                this.Modelview = modelview;
            }

            /// <summary>
            /// TransformMatrixSetクラスのインスタンスを初期化する。
            /// </summary>
            public TransformMatrixes()
            {
            }


            #region public static TransformMatrixes Current [get]

            /// <summary>
            /// ビューポート、プロジェクション行列、モデルビュー行列を取得・設定する。
            /// </summary>
            public static TransformMatrixes Current
            {
                get
                {
                    int[] current_viewport = new int[4];
                    Gl.glGetIntegerv(Gl.GL_VIEWPORT, current_viewport);
                    double[] current_projection;
                    current_projection = new double[16];
                    Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, current_projection);
                    double[] current_modelview;
                    current_modelview = new double[16];
                    Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, current_modelview);
                    return new TransformMatrixes(current_viewport, current_projection, current_modelview);
                }
                set
                {
                    int prev_mode;
                    Gl.glGetIntegerv(Gl.GL_MATRIX_MODE,out prev_mode);
                    
                    Gl.glViewport(value.viewport[0], value.viewport[1], value.viewport[2], value.viewport[3]);
                   
                    Gl.glMatrixMode(Gl.GL_PROJECTION);
                    Gl.glLoadMatrixd(value.projection);
                    

                    Gl.glMatrixMode(Gl.GL_MODELVIEW);
                    Gl.glLoadMatrixd(value.modelview);

                    Gl.glMatrixMode(prev_mode);
                }
            }

            #endregion Current


            #region public void GetCurrent()

            /// <summary>
            /// 現在のビューポート、プロジェクション行列、モデルビュー行列を取得する。
            /// </summary>
            public void GetCurrent()
            {

                int[] current_viewport = new int[4];
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, current_viewport);
                double[] current_projection = new double[16];
                Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, current_projection);
                double[] current_modelview = new double[16];
                Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, current_modelview);
                current_viewport.CopyTo(this.viewport, 0);
                current_projection.CopyTo(this.projection, 0);
                current_modelview.CopyTo(this.modelview, 0);
            }

            #endregion


            #region public double[] UnProject(...)

            /// <summary>
            /// ウィンドウ座標とそのデプス値をワールド座標へ変換する。
            /// </summary>
            /// <param name="winX"></param>
            /// <param name="winY"></param>
            /// <param name="depth"></param>
            /// <returns></returns>
            public double[] UnProject(int winX, int winY, double depth)
            {
                double objx = 0d, objy = 0d, objz = 0d;
                 Glu.gluUnProject(winX, winY, depth, this.modelview, this.projection, this.viewport, out objx, out objy, out objz);
                
                return new double[] { objx, objy, objz };
            }

            /// <summary>
            /// ウィンドウ座標をワールド座標へ変換する。
            /// <para>デプス値は、デプスバッファからglReadPixels関数で取得される。</para>
            /// </summary>
            /// <param name="winX"></param>
            /// <param name="winY"></param>
            /// <param name="depth"></param>
            /// <returns></returns>
            public double[] UnProject(int winX, int winY, out float depth)
            {
                float[] pixels = new float[1];
                Gl.glReadPixels(winX, winY, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, pixels);
                depth = pixels[0];

                return this.UnProject(winX, winY, depth);
            }

            #endregion UnProject


            #region public double[] Project(...)

            /// <summary>
            /// ワールド座標をウィンドウ座標へ変換する。
            /// </summary>
            /// <param name="coord"></param>
            /// <returns></returns>
            public double[] Project(double[] coord)
            {
                double winx = 0, winy = 0, depth = 0;
                Glu.gluProject(coord[0], coord[1], coord[2], this.modelview, this.projection, this.viewport, out winx, out winy, out depth);
                return new double[] { winx, winy, depth };
            }

            /// <summary>
            /// ワールド座標をウィンドウ座標へ変換する。
            /// </summary>
            /// <param name="coord"></param>
            /// <param name="winX"></param>
            /// <param name="winY"></param>
            /// <param name="depth"></param>
            public void Project(double[] coord, out int winX, out int winY, out double depth)
            {
                double x = 0d, y = 0d;
                depth = 0d;
                Glu.gluProject(coord[0], coord[1], coord[2], this.modelview, this.projection, this.viewport, out x, out y, out depth);
                winX = (int)x;
                winY = (int)y;
            }

            #endregion Project


            #region ICloneable メンバ

            public object Clone()
            {
                return new TransformMatrixes(this.viewport, this.projection, this.modelview);
            }

            #endregion

        }

        #endregion class TransformMatrixSet


        //#######################################
        //#######################################


        #region interface ISelectable

        /// <summary>
        /// マウスピッキングのための機能を提供する。
        /// </summary>
        public interface ISelectable
        {
            /// <summary>
            /// 選択可能かどうかを取得する。
            /// </summary>
            bool Selectable { get; }
            uint name { get; set;}
            /// <summary>
            /// <para>セレクションモードで描画を行う。</para>
            /// </summary>
            /// <param name="rp">レンダリングパラメータ</param>
            /// <param name="name">セレクションモードでこのオブジェクトに割り当てられた識別番号。（glLoadNameで割り当てられる名前。）</param>
            /// <remarks>
            /// <para>実装時は、このメソッド内でglPushName、glLoadName、glPopNameを使用して
            /// 子オブジェクトを個別に識別・ヒットさせることができる。</para>
            /// </remarks>
            void DrawSceneForSelectionMode(uint name);

            /// <summary>
            /// <para>与えられたselectionDataに基づいて、ヒットしたオブジェクトを返す。</para>
            /// </summary>
            /// <param name="selectionData">セレクションデータ（セレクションバッファに返ってきた値）</param>
            /// <returns></returns>
            /// <remarks>
            /// <para>実装時は、子オブジェクトを持たない場合はこのオブジェクト自身を返せばよい。</para>
            /// <para>子オブジェクトを持つ場合、selectionData.names[0]にこのオブジェクトの識別番号（glLoadNameで割り当てられる名前）が格納されており、
            /// selectionData.names[1]以降の要素に子オブジェクトの識別番号が格納されている。</para>
            /// </remarks>
            ISelectable GettHitObject(SelectionData selectionData);

        }

        #endregion


        /// <summary>
        /// マウスピッキングを行うためのクラス
        /// </summary>
        public static class Selection
        {

            #region public static int SelectionBufferLength [get,set]

            /// <summary>
            /// PickModelメソッド内部で使用されるセレクションバッファの長さ。
            /// デフォルトは100。
            /// </summary>
            private static int selectionBufferLength = 200;

            /// <summary>
            /// <para>[get,set] Pickメソッド内部で使用されるセレクションバッファの長さ。</para>
            /// <para>デフォルトは100。</para>
            /// </summary>
            public static int SelectionBufferLength
            {
                get { return Selection.selectionBufferLength; }
                set { Selection.selectionBufferLength = value; }
            }

            #endregion


            //#######################################


            #region public static List<SelectionData> Pick<T>( ... ) where T : class

            /// <summary>
            /// OpenGLのセレクションモードを利用して、マウスピッキングを行う。
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="items">対象となるオブジェクトのリスト</param>
            /// <param name="renderingParams">レンダリングパラメータ</param>
            /// <param name="mousePos">ピッキングするウィンドウ座標（左下原点）（double[]{x, y}）</param>
            /// <param name="region">ヒットする範囲の幅と高さ（double[]{width, height}）</param>
            /// <param name="camera">シーンのカメラ（要プロジェクション設定）</param>
            /// <param name="viewport">シーンのビューポート</param>
            /// <param name="pickChild">trueを指定した場合､子オブジェクト単位でヒットする。
            /// falseの場合は、階層構造のトップのオブジェクトがヒットする。</param>
            /// <param name="pickedMatrixSet">セレクションモードで使用された変換行列。
            /// ただし、Modelviewプロパティには引数cameraによるビューイング変換を表す行列が格納される。</param>
            /// <returns><para>ヒットしたオブジェクトのリスト。</para>
            /// <para>ヒットしたオブジェクトは、SelectionData.itemフィールドに格納されている。</para></returns>
            public static List<SelectionData> Pick<T>(
                IDictionary<int,T> items,
                double[] mousePos, double[] region, Jusin.Camera.CCamera camera, int[] viewport, bool pickChild,
                out TransformMatrixes pickedMatrixSet)
                where T : class
            {
               
                int err = Gl.glGetError();
                if (err != Gl.GL_NO_ERROR)
                {
                    
                    throw new Exception();
                }

                uint[] selectionBuff = new uint[SelectionBufferLength];
                Gl.glSelectBuffer(selectionBuff.Length, selectionBuff);

                Gl.glRenderMode(Gl.GL_SELECT);

                Gl.glInitNames();
                Gl.glPushName(0);

                //マウスピッキング時の変換行列
                //しばしば必要になるので、取得しておく。
                pickedMatrixSet = new TransformMatrixes();
                pickedMatrixSet.Viewport = viewport;

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();
                Gl.glLoadIdentity();

                //プロジェクション行列を取得。
               // camera.ApplyProjection();
                double[] current_projection = new double[16];
                Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, current_projection);
                pickedMatrixSet.Projection = current_projection;

                //PickMatrixをセットする必要があるので、改めてプロジェクション行列をセット。
                Gl.glLoadIdentity();
                Glu.gluPickMatrix(mousePos[0], mousePos[1], region[0], region[1], viewport);
                Gl.glMultMatrixd(pickedMatrixSet.Projection);
                
                //モデルビュー行列を設定。
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                Gl.glLoadIdentity();
                camera.ApplyProjection();
                camera.gluLookAtLH();

                //モデルビュー行列を取得。
                Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, pickedMatrixSet.Modelview);


                foreach (int key in items.Keys)
                {
                    if( (items[key]!=null)&&(items[key] is ISelectable) )
                    {
                        ISelectable drawItem = (ISelectable)items[key];
                        if (drawItem.Selectable)
                        {
                            Gl.glLoadName(drawItem.name);
                            drawItem.DrawSceneForSelectionMode(drawItem.name);
                        }
                    }
                }/*
                for (int i = 0; i < items.Count; i++)
                {
                    
                    if ((items[i] != null) && (items[i] is ISelectable))
                    {
                        ISelectable drawItem = (ISelectable)items[i];
                        if (drawItem.Selectable)
                        {
                            Gl.glLoadName(drawItem.name);
                            drawItem.DrawSceneForSelectionMode(drawItem.name);
                        }
                    }
                }*/

                Gl.glPopName();

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPopMatrix();

                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPopMatrix();

                int hits = Gl.glRenderMode(Gl.GL_RENDER);

                err = Gl.glGetError();
                if (err != Gl.GL_NO_ERROR)
                {
                    throw new Exception(err.ToString());
                }

                List<SelectionData> hitData = SelectionData.ParseSelectionBuffer(selectionBuff);

                if (hitData.Count != 0)
                {
                    if (pickChild)
                    {
                        for (int i = 0; i < hitData.Count; i++)
                        {
                            hitData[i].item = ((ISelectable)items[hitData[i].names[0]]).GettHitObject(hitData[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < hitData.Count; i++)
                        {
                            hitData[i].item = (ISelectable)items[hitData[i].names[0]];
                        }
                    }
                }

                return hitData;
            }

            #endregion



            #region public static SelectionData GetNearest(...)

            /// <summary>
            /// <para>ヒットしたオブジェクトのリストから、最も手前にあるオブジェクトを取得する。</para>
            /// <para>リストのカウントが0の場合はnullを返す。</para>
            /// </summary>
            /// <param name="pickedList">ヒットしたオブジェクトのリスト</param>
            /// <returns><para>最も手前にあるオブジェクト。</para>
            /// <para>pickedListのカウントが0の場合はnullを返す。</para></returns>
            public static SelectionData GetNearest(List<SelectionData> pickedList)
            {
                if (pickedList == null) { return null; }

                double zMin = double.MaxValue;
                SelectionData nearest = null;
                for (int i = 0; i < pickedList.Count; i++)
                {
                    if (pickedList[i].zMin < zMin)
                    {
                        nearest = pickedList[i];
                    }
                }

                return nearest;
            }


            /// <summary>
            /// <para>ヒットしたオブジェクトのリストから、最も手前にあるオブジェクトを取得する。</para>
            /// <para>リストのカウントが0の場合はnullを返す。</para>
            /// </summary>
            /// <param name="pickedList">ヒットしたオブジェクトのリスト</param>
            /// <param name="depth">ヒットしたデプス値が返される。</param>
            /// <returns><para>最も手前にあるオブジェクト。</para>
            /// <para>pickedListのカウントが0の場合はnullを返す。</para></returns>
            public static ISelectable GetNearest(List<SelectionData> pickedList, out double depth)
            {
                if (pickedList == null)
                {
                    depth = 1.0;
                    return null;
                }

                depth = double.MaxValue;
                SelectionData nearest = null;
                for (int i = 0; i < pickedList.Count; i++)
                {
                    if (pickedList[i].zMin < depth)
                    {
                        nearest = pickedList[i];
                        depth = pickedList[i].zMin;
                    }
                }

                if (nearest == null)
                {
                    depth = 1.0;
                    return null;
                }

                return nearest.item;
            }

            #endregion


            #region public static SelectionData GetFarthest(...)

            /// <summary>
            /// <para>ヒットしたオブジェクトのリストから、最も遠くにあるオブジェクトを取得する。</para>
            /// <para>リストのカウントが0の場合はnullを返す。</para>
            /// </summary>
            /// <param name="pickedList">ヒットしたオブジェクトのリスト</param>
            /// <returns><para>最も遠くにあるオブジェクト。</para>
            /// <para>pickedListのカウントが0の場合はnullを返す。</para></returns>
            public static SelectionData GetFarthest(List<SelectionData> pickedList)
            {
                if (pickedList == null) { return null; }

                double zMax = double.MinValue;
                SelectionData farthest = null;
                for (int i = 0; i < pickedList.Count; i++)
                {
                    if (pickedList[i].zMax > zMax)
                    {
                        farthest = pickedList[i];
                    }
                }

                return farthest;
            }


            /// <summary>
            /// <para>ヒットしたオブジェクトのリストから、最も遠くにあるオブジェクトを取得する。</para>
            /// <para>リストのカウントが0の場合はnullを返す。</para>
            /// </summary>
            /// <param name="pickedList">ヒットしたオブジェクトのリスト</param>
            /// <param name="depth">ヒットしたデプス値が返される。</param>
            /// <returns><para>最も遠くにあるオブジェクト。</para>
            /// <para>pickedListのカウントが0の場合はnullを返す。</para></returns>
            public static ISelectable GetFarthest(List<SelectionData> pickedList, out double depth)
            {
                if (pickedList == null)
                {
                    depth = 1.0;
                    return null;
                }

                depth = double.MinValue;
                SelectionData farthest = null;
                for (int i = 0; i < pickedList.Count; i++)
                {
                    if (pickedList[i].zMax > depth)
                    {
                        farthest = pickedList[i];
                        depth = pickedList[i].zMax;
                    }
                }

                if (farthest == null)
                {
                    depth = 1.0;
                    return null;
                }

                return farthest.item;
            }

            #endregion


        }


}
