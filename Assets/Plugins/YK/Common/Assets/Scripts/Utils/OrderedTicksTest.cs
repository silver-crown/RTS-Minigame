namespace moveen.utils {
    public class OrderedTicksTest {
        private int counter1;
        private int counter2;

        public bool haveToUpdate() {
            return true;
        }

        public bool haveToUpdateAfterFixedUpdate() {
            if (counter1 == 0) {
                counter1 = 5;
                return false;
            }
            counter1--;
            return true;
        }

        public bool haveToFixedUpdate() {
            return true;
        }
    }
}