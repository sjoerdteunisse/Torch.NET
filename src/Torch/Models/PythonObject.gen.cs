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
    public partial class PythonObject
    {
        
        
        //auto-generated
        public PyTuple ToTuple(Array input)
        {
            var array = new PyObject[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                array[i]=ToPython(input.GetValue(i));
            }
            return new PyTuple(array);
        }
        
        //auto-generated
        public PyObject ToPython(object obj)
        {
            if (obj == null) return Runtime.GetPyNone();
            switch (obj)
            {
                // basic types
                case int o: return new PyInt(o);
                case long o: return new PyLong(o);
                case float o: return new PyFloat(o);
                case double o: return new PyFloat(o);
                case string o: return new PyString(o);
                case bool o: return ConverterExtension.ToPython(o);
                case PyObject o: return o;
                // sequence types
                case Array o: return ToTuple(o);
                // special types from 'ToPythonConversions'
                case Shape o: return ToTuple(o.Dimensions);
                case Torch.PythonObject o: return o.PyObject;
                case Numpy.PythonObject o: return o.PyObject;
                default: throw new NotImplementedException($"Type is not yet supported: { obj.GetType().Name}. Add it to 'ToPythonConversions'");
            }
        }
        
        //auto-generated
        public T ToCsharp<T>(dynamic pyobj)
        {
            switch (typeof(T).Name)
            {
                // types from 'ToCsharpConversions'
                case "Tensor": return (T)(object)new Tensor(pyobj);
                case "Dtype": return (T)(object)new Dtype(pyobj);
                case "Layout": return (T)(object)new Layout(pyobj);
                case "Device": return (T)(object)new Device(pyobj);
                case "NDarray": return (T)(object)new NDarray(pyobj);
                case "Storage": return (T)(object)new Storage(pyobj);
                case "Shape": return (T)(object)new Shape(pyobj.As<int[]>());
                default:
                try
                {
                    return pyobj.As<T>();
                }
                catch (Exception e)
                {
                    throw new NotImplementedException($"conversion from {typeof(T).Name} to {pyobj.__class__} not implemented", e);
                    return default(T);
                }
            }
        }
        
        //auto-generated
        public T SharpToSharp<T>(object obj)
        {
            if (obj == null) return default(T);
            switch (obj)
            {
                // from 'SharpToSharpConversions':
            }
            throw new NotImplementedException($"Type is not yet supported: { obj.GetType().Name}. Add it to 'SharpToSharpConversions'");
        }
    }
}
