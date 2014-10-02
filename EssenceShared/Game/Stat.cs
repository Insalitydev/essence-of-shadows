namespace EssenceShared.Game {
    public class Stat {
        public int Current;
        public int Maximum;

        public Stat(int max) {
            Maximum = max;
            Current = Maximum;
        }

        public float GetPerc() {
            if (Maximum == 0){
                return 1;
            }

            var perc = (Current/(float) Maximum);

            if (perc >= 0){
                return perc;
            }
            return 0;
        }
    }
}