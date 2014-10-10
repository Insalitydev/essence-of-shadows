namespace EssenceShared.Game {
    public class Stat {
        private int _maximum;
        public int Maximum {
            get { return _maximum;  }
            set {
                var perc = Perc;
                _maximum = value;
                Current = (int)(_maximum*perc);
            }
        } 
        private int _current;

        public Stat(int max) {
            Maximum = max;
            Current = Maximum;
        }

        public int Current {
            get { return _current; }
            set {
                _current = value;
                if (Current > Maximum) Current = Maximum;
                if (Current < 0) Current = 0;
            }
        }

        /** Возвращает текущий процент характеристики */

        public float Perc {
            get {
                if (Maximum == 0){
                    return 1;
                }

                float perc = (Current/(float) Maximum);

                if (perc >= 0){
                    return perc;
                }
                return 0;
            }
        }
    }
}