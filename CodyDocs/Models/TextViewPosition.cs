using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs
{
//public struct TextViewPosition
//{
//    private readonly int _column;
//    private readonly int _line;

//    public TextViewPosition(int line, int column)
//    {
//        _line = line;
//        _column = column;
//    }

//    public int Line { get { return _line; } }
//    public int Column { get { return _column; } }


//    public static bool operator <(TextViewPosition a, TextViewPosition b)
//    {
//        if (a.Line < b.Line)
//        {
//            return true;
//        }
//        else if (a.Line == b.Line)
//        {
//            return a.Column < b.Column;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public static bool operator >(TextViewPosition a, TextViewPosition b)
//    {
//        if (a.Line > b.Line)
//        {
//            return true;
//        }
//        else if (a.Line == b.Line)
//        {
//            return a.Column > b.Column;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public static TextViewPosition Min(TextViewPosition a, TextViewPosition b)
//    {
//        return a > b ? b : a;
//    }

//    public static TextViewPosition Max(TextViewPosition a, TextViewPosition b)
//    {
//        return a > b ? a : b;
//    }

//        //public bool IsEmpty { get { return Line <= 0 && Column <= 0; } }

//    public bool Equals(TextViewPosition other)
//    {
//        return _column == other._column && _line == other._line;
//    }

//        //public override bool Equals(object obj)
//        //{
//        //    if (ReferenceEquals(null, obj))
//        //    {
//        //        return false;
//        //    }
//        //    return obj is TextViewPosition && Equals((TextViewPosition)obj);
//        //}

//        //public override int GetHashCode()

//        //{
//        //    unchecked
//        //    {
//        //        return (_column * 397) ^ _line;
//        //    }
//        //}

//        //public static bool operator ==(TextViewPosition left, TextViewPosition right)
//        //{
//        //    return left.Equals(right);
//        //}

//        //public static bool operator <=(TextViewPosition left, TextViewPosition right)
//        //{
//        //    return !(left > right);
//        //}

//        //public static bool operator >=(TextViewPosition left, TextViewPosition right)
//        //{
//        //    return !(left < right);
//        //}



//        //public static bool operator !=(TextViewPosition left, TextViewPosition right)
//        //{
//        //    return !left.Equals(right);
//        //}


//    }
}
