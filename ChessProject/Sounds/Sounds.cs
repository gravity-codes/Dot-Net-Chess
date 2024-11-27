using System.Media;

namespace ChessLogic
{
    public class Sounds
    {
        SoundPlayer Capture = new SoundPlayer("Sounds/capture.wav");
        SoundPlayer Castle = new SoundPlayer("Sounds/castle.wav");
        SoundPlayer Check = new SoundPlayer("Sounds/move-check.wav");
        SoundPlayer Move = new SoundPlayer("Sounds/move-self.wav");
        SoundPlayer Notify = new SoundPlayer("Sounds/notify.wav");
        SoundPlayer Promotion = new SoundPlayer("Sounds/promote.wav");

        public void PlayMoveSound(Move move)
        {
            switch (move.Type)
            {
                
            }
        }
    }
}
