namespace EssenceShared.Game {
    public class Stat {
        private int _current;
        public int Current {
            get { return _current; }
            set {
                _current = value;
                if (Current > Maximum) Current = Maximum;
                if (Current < 0) Current = 0;
            }
        }
        public int Maximum;

        public Stat(int max) {
            Maximum = max;
            Current = Maximum;
        }

        /** Возвращает текущий процент характеристики */
        public float Perc {
            get {
                if (Maximum == 0) {
                    return 1;
                }

                float perc = (Current / (float)Maximum);

                if (perc >= 0) {
                    return perc;
                }
                return 0;
            }
        }
    }
}