// Title:	Triangle.cs
// Author: 	Scott Ellington <scott.ellington@gmail.com>
//
// Copyright (C) 2006 Scott Ellington and authors
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace ThirdParty.ThreeDLib.ThreeD
{
	public class Triangle : IComparable
	{
		public int vertex1;
		public int vertex2;
		public int vertex3;
        public double dist;
		public Triangle ( int v1, int v2, int v3 )
		{
			vertex1 = v1;
			vertex2 = v2;
			vertex3 = v3;
            dist = 1.0;
		}

		public override string ToString ()
		{
			return String.Format ( "v1: {0} v2: {1} v3: {2}", vertex1, vertex2, vertex3 );
		}
        public int CompareTo(object obj)
        {
            double ret = -(this.dist - ((Triangle)obj).dist);
            return (int)ret;
        }
      
	}
}

