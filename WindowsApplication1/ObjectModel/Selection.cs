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
        /// �Z���N�V�������[�h�Ńq�b�g�����I�u�W�F�N�g�̃f�[�^��\���B
        /// </summary>
        public class SelectionData
        {
            /// <summary>
            /// ���ʔԍ��̊K�w�̐[��
            /// </summary>
            public int nameStackDepth;

            /// <summary>
            /// �q�b�g�����I�u�W�F�N�g�̐[�x�̍ő�l
            /// </summary>
            public double zMax;
            /// <summary>
            /// �q�b�g�����I�u�W�F�N�g�̐[�x�̍ŏ��l
            /// </summary>
            public double zMin;

            /// <summary>
            /// �q�b�g�����I�u�W�F�N�g�̎��ʔԍ�
            /// </summary>
            public int[] names;


            /// <summary>
            /// �q�b�g�����I�u�W�F�N�g
            /// </summary>
            public ISelectable item;


            #region ParseSelectionBuffer

            /// <summary>
            /// uint[]�z��̃Z���N�V�����E�o�b�t�@����͂��āA
            /// SelectionData�̃��X�g�ɕϊ�����B
            /// </summary>
            /// <param name="selectionBuffer">�Z���N�V�������[�h�œ���ꂽ�Z���N�V�����E�o�b�t�@</param>
            /// <returns>�q�b�g�����f�[�^�̃��X�g</returns>
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
                catch (IndexOutOfRangeException ex)�@//selectionBuffer�̒���������Ȃ��ƁAIndexOutOfRangeException���������Ă����ɗ���B
                {
                    if (i >= selectionBuffer.Length) // �܂�AselectionBuffer�̒���������Ȃ�
                    {
                        //���̏ꍇ�́A���܂��擾�ł����f�[�^������Ԃ��āA����������Ȃ����͖����B
                    }
                    else
                    {   //����ȊO�͑z��O�̃G���[�Ȃ̂ŁA�Ƃ肠�����G���[�𓊂��Ă����B
                        throw new Exception("ParseSelectionBuffer failed.", ex);
                    }
                }
                catch (Exception ex)
                {   //IndexOutOfRangeException�ȊO�͑z��O�̃G���[�Ȃ̂ŁA�Ƃ肠�����G���[�𓊂��Ă����B
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
        /// <para>�r���[�|�[�g�A�v���W�F�N�V�����s��A���f���r���[�s��̃Z�b�g</para>
        /// <para>OpenGL�Ŏg�p����ϊ��s��̃Z�b�g�B</para>
        /// </summary>
        public class TransformMatrixes : ICloneable
        {

            #region �v���p�e�B


            #region int[] Viewport

            /// <summary>
            /// �r���[�|�[�g
            /// </summary>
            private int[] viewport = new int[] { 0, 0, 100, 100 };

            /// <summary>
            /// �r���[�|�[�g���擾�E�ݒ肷��B
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
            /// �v���W�F�N�V�����s��
            /// </summary>
            /// 
            private double[] projection = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            /// <summary>
            /// �v���W�F�N�V�����s����擾�E�ݒ肷��B
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
            /// ���f���r���[�s��
            /// </summary>
            private double[] modelview = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            /// <summary>
            /// ���f���r���[�s����擾�E�ݒ肷��B
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


            #endregion �v���p�e�B


            /// <summary>
            /// TransformMatrixes�N���X�̃C���X�^���X������������B
            /// </summary>
            /// <param name="viewport">�r���[�|�[�g</param>
            /// <param name="projecton">�v���W�F�N�V�����s��</param>
            /// <param name="modelview">���f���r���[�s��</param>
            public TransformMatrixes(int[] viewport, double[] projecton, double[] modelview)
            {
                this.Viewport = viewport;
                this.Projection = projecton;
                this.Modelview = modelview;
            }

            /// <summary>
            /// TransformMatrixSet�N���X�̃C���X�^���X������������B
            /// </summary>
            public TransformMatrixes()
            {
            }


            #region public static TransformMatrixes Current [get]

            /// <summary>
            /// �r���[�|�[�g�A�v���W�F�N�V�����s��A���f���r���[�s����擾�E�ݒ肷��B
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
            /// ���݂̃r���[�|�[�g�A�v���W�F�N�V�����s��A���f���r���[�s����擾����B
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
            /// �E�B���h�E���W�Ƃ��̃f�v�X�l�����[���h���W�֕ϊ�����B
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
            /// �E�B���h�E���W�����[���h���W�֕ϊ�����B
            /// <para>�f�v�X�l�́A�f�v�X�o�b�t�@����glReadPixels�֐��Ŏ擾�����B</para>
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
            /// ���[���h���W���E�B���h�E���W�֕ϊ�����B
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
            /// ���[���h���W���E�B���h�E���W�֕ϊ�����B
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


            #region ICloneable �����o

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
        /// �}�E�X�s�b�L���O�̂��߂̋@�\��񋟂���B
        /// </summary>
        public interface ISelectable
        {
            /// <summary>
            /// �I���\���ǂ������擾����B
            /// </summary>
            bool Selectable { get; }
            uint name { get; set;}
            /// <summary>
            /// <para>�Z���N�V�������[�h�ŕ`����s���B</para>
            /// </summary>
            /// <param name="rp">�����_�����O�p�����[�^</param>
            /// <param name="name">�Z���N�V�������[�h�ł��̃I�u�W�F�N�g�Ɋ��蓖�Ă�ꂽ���ʔԍ��B�iglLoadName�Ŋ��蓖�Ă��閼�O�B�j</param>
            /// <remarks>
            /// <para>�������́A���̃��\�b�h����glPushName�AglLoadName�AglPopName���g�p����
            /// �q�I�u�W�F�N�g���ʂɎ��ʁE�q�b�g�����邱�Ƃ��ł���B</para>
            /// </remarks>
            void DrawSceneForSelectionMode(uint name);

            /// <summary>
            /// <para>�^����ꂽselectionData�Ɋ�Â��āA�q�b�g�����I�u�W�F�N�g��Ԃ��B</para>
            /// </summary>
            /// <param name="selectionData">�Z���N�V�����f�[�^�i�Z���N�V�����o�b�t�@�ɕԂ��Ă����l�j</param>
            /// <returns></returns>
            /// <remarks>
            /// <para>�������́A�q�I�u�W�F�N�g�������Ȃ��ꍇ�͂��̃I�u�W�F�N�g���g��Ԃ��΂悢�B</para>
            /// <para>�q�I�u�W�F�N�g�����ꍇ�AselectionData.names[0]�ɂ��̃I�u�W�F�N�g�̎��ʔԍ��iglLoadName�Ŋ��蓖�Ă��閼�O�j���i�[����Ă���A
            /// selectionData.names[1]�ȍ~�̗v�f�Ɏq�I�u�W�F�N�g�̎��ʔԍ����i�[����Ă���B</para>
            /// </remarks>
            ISelectable GettHitObject(SelectionData selectionData);

        }

        #endregion


        /// <summary>
        /// �}�E�X�s�b�L���O���s�����߂̃N���X
        /// </summary>
        public static class Selection
        {

            #region public static int SelectionBufferLength [get,set]

            /// <summary>
            /// PickModel���\�b�h�����Ŏg�p�����Z���N�V�����o�b�t�@�̒����B
            /// �f�t�H���g��100�B
            /// </summary>
            private static int selectionBufferLength = 200;

            /// <summary>
            /// <para>[get,set] Pick���\�b�h�����Ŏg�p�����Z���N�V�����o�b�t�@�̒����B</para>
            /// <para>�f�t�H���g��100�B</para>
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
            /// OpenGL�̃Z���N�V�������[�h�𗘗p���āA�}�E�X�s�b�L���O���s���B
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="items">�ΏۂƂȂ�I�u�W�F�N�g�̃��X�g</param>
            /// <param name="renderingParams">�����_�����O�p�����[�^</param>
            /// <param name="mousePos">�s�b�L���O����E�B���h�E���W�i�������_�j�idouble[]{x, y}�j</param>
            /// <param name="region">�q�b�g����͈͂̕��ƍ����idouble[]{width, height}�j</param>
            /// <param name="camera">�V�[���̃J�����i�v�v���W�F�N�V�����ݒ�j</param>
            /// <param name="viewport">�V�[���̃r���[�|�[�g</param>
            /// <param name="pickChild">true���w�肵���ꍇ��q�I�u�W�F�N�g�P�ʂŃq�b�g����B
            /// false�̏ꍇ�́A�K�w�\���̃g�b�v�̃I�u�W�F�N�g���q�b�g����B</param>
            /// <param name="pickedMatrixSet">�Z���N�V�������[�h�Ŏg�p���ꂽ�ϊ��s��B
            /// �������AModelview�v���p�e�B�ɂ͈���camera�ɂ��r���[�C���O�ϊ���\���s�񂪊i�[�����B</param>
            /// <returns><para>�q�b�g�����I�u�W�F�N�g�̃��X�g�B</para>
            /// <para>�q�b�g�����I�u�W�F�N�g�́ASelectionData.item�t�B�[���h�Ɋi�[����Ă���B</para></returns>
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

                //�}�E�X�s�b�L���O���̕ϊ��s��
                //���΂��ΕK�v�ɂȂ�̂ŁA�擾���Ă����B
                pickedMatrixSet = new TransformMatrixes();
                pickedMatrixSet.Viewport = viewport;

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();
                Gl.glLoadIdentity();

                //�v���W�F�N�V�����s����擾�B
               // camera.ApplyProjection();
                double[] current_projection = new double[16];
                Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, current_projection);
                pickedMatrixSet.Projection = current_projection;

                //PickMatrix���Z�b�g����K�v������̂ŁA���߂ăv���W�F�N�V�����s����Z�b�g�B
                Gl.glLoadIdentity();
                Glu.gluPickMatrix(mousePos[0], mousePos[1], region[0], region[1], viewport);
                Gl.glMultMatrixd(pickedMatrixSet.Projection);
                
                //���f���r���[�s���ݒ�B
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                Gl.glLoadIdentity();
                camera.ApplyProjection();
                camera.gluLookAtLH();

                //���f���r���[�s����擾�B
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
            /// <para>�q�b�g�����I�u�W�F�N�g�̃��X�g����A�ł���O�ɂ���I�u�W�F�N�g���擾����B</para>
            /// <para>���X�g�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para>
            /// </summary>
            /// <param name="pickedList">�q�b�g�����I�u�W�F�N�g�̃��X�g</param>
            /// <returns><para>�ł���O�ɂ���I�u�W�F�N�g�B</para>
            /// <para>pickedList�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para></returns>
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
            /// <para>�q�b�g�����I�u�W�F�N�g�̃��X�g����A�ł���O�ɂ���I�u�W�F�N�g���擾����B</para>
            /// <para>���X�g�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para>
            /// </summary>
            /// <param name="pickedList">�q�b�g�����I�u�W�F�N�g�̃��X�g</param>
            /// <param name="depth">�q�b�g�����f�v�X�l���Ԃ����B</param>
            /// <returns><para>�ł���O�ɂ���I�u�W�F�N�g�B</para>
            /// <para>pickedList�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para></returns>
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
            /// <para>�q�b�g�����I�u�W�F�N�g�̃��X�g����A�ł������ɂ���I�u�W�F�N�g���擾����B</para>
            /// <para>���X�g�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para>
            /// </summary>
            /// <param name="pickedList">�q�b�g�����I�u�W�F�N�g�̃��X�g</param>
            /// <returns><para>�ł������ɂ���I�u�W�F�N�g�B</para>
            /// <para>pickedList�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para></returns>
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
            /// <para>�q�b�g�����I�u�W�F�N�g�̃��X�g����A�ł������ɂ���I�u�W�F�N�g���擾����B</para>
            /// <para>���X�g�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para>
            /// </summary>
            /// <param name="pickedList">�q�b�g�����I�u�W�F�N�g�̃��X�g</param>
            /// <param name="depth">�q�b�g�����f�v�X�l���Ԃ����B</param>
            /// <returns><para>�ł������ɂ���I�u�W�F�N�g�B</para>
            /// <para>pickedList�̃J�E���g��0�̏ꍇ��null��Ԃ��B</para></returns>
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
