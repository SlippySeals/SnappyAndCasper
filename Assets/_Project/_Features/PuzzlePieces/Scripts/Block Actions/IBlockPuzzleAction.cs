
using SnappyNCasper.PuzzlePieces.PressurePlate;

namespace SnappyNCasper.PuzzlePieces.BlockActions
{
    public interface IBlockPuzzleAction
    {
        public void DoBehaviour(PlateState state);
        // should we do the behaviour
    }
}