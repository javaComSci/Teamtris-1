using System;
using System.Collections.Generic;
using System.Linq;

public class CompatiblePiece {
    public int area { get; set; }
    public List<Tuple<int, int>> locationOnBoard { get; set; }
    public int numLinesCleared { get; set; }
    
    public CompatiblePiece(List<Tuple<int, int>> locationOnBoard, int area, int numLinesCleared) {
        this.area = area;
        this.locationOnBoard = locationOnBoard;
        this.numLinesCleared = numLinesCleared;
    }
}