using System;
using System.Collections.Generic;
using System.Linq;


// #code BotDefVar csharp
public abstract class Bot {
    public abstract List<Tuple<int, int>> GetMove(
        Board board, 
        List<List<Block>> allBotBlocks, 
        bool allRotations = false
    );
}