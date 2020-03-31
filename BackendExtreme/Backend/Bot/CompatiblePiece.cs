using System;
using System.Collections.Generic;
using System.Linq;

public class CompatiblePiece : IComparable {
    public int area { get; set; }
    public List<Tuple<int, int>> locationOnBoard { get; set; }
    public int numLinesCleared { get; set; }
    
    public CompatiblePiece(List<Tuple<int, int>> locationOnBoard, int area, int numLinesCleared) {
        this.area = area;
        this.locationOnBoard = locationOnBoard;
        this.numLinesCleared = numLinesCleared;
    }

     public int CompareTo(Object obj) {
        CompatiblePiece x = this;
        CompatiblePiece y  = (CompatiblePiece)obj;
        // sort by whether a line has been cleared and puts those first if there is
        int result = y.numLinesCleared.CompareTo(x.numLinesCleared);
        // sort by the area covered
        return result == 0 ? y.area.CompareTo(x.area) : result;
    }
}