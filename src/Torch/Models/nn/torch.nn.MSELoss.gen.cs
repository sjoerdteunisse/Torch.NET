// Code generated by CodeMinion: https://github.com/SciSharp/CodeMinion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Python.Runtime;
using Numpy;
using Numpy.Models;

namespace Torch
{
    public static partial class torch {
        public static partial class nn {
            /// <summary>
            ///	Creates a criterion that measures the mean squared error (squared L2 norm) between
            ///	each element in the input \(x\) and target \(y\).<br></br>
            ///	
            ///	The unreduced (i.e.<br></br>
            ///	 with reduction set to 'none') loss can be described as:
            ///	
            ///	\[\ell(x, y) = L = \{l_1,\dots,l_N\}^\top, \quad
            ///	l_n = \left( x_n - y_n \right)^2,
            ///	
            ///	\]
            ///	
            ///	where \(N\) is the batch size.<br></br>
            ///	 If reduction is not 'none'
            ///	(default 'mean'), then:
            ///	
            ///	\[\ell(x, y) =
            ///	\begin{cases}
            ///	    \operatorname{mean}(L), &  \text{if reduction} = \text{'mean';}\\
            ///	    \operatorname{sum}(L),  &  \text{if reduction} = \text{'sum'.}
            ///	\end{cases}
            ///	
            ///	\]
            ///	
            ///	\(x\) and \(y\) are tensors of arbitrary shapes with a total
            ///	of \(n\) elements each.<br></br>
            ///	
            ///	The sum operation still operates over all the elements, and divides by \(n\).<br></br>
            ///	
            ///	The division by \(n\) can be avoided if one sets reduction = 'sum'.
            /// </summary>
            public partial class MSELoss : Module
            {
                // auto-generated class
                
                public MSELoss(PyObject pyobj) : base(pyobj) { }
                
                public MSELoss(Module other) : base(other.PyObject as PyObject) { }
                
                public MSELoss(bool? size_average = null, bool? reduce = null, string reduction = "mean")
                {
                    //auto-generated code, do not change
                    var nn = self.GetAttr("nn");
                    var __self__=nn;
                    var pyargs=ToTuple(new object[]
                    {
                    });
                    var kwargs=new PyDict();
                    if (size_average!=null) kwargs["size_average"]=ToPython(size_average);
                    if (reduce!=null) kwargs["reduce"]=ToPython(reduce);
                    if (reduction!="mean") kwargs["reduction"]=ToPython(reduction);
                    dynamic py = __self__.InvokeMethod("MSELoss", pyargs, kwargs);
                    self=py as PyObject;
                }
                
            }
        }
    }
    
}
