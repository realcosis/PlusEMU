namespace Plus.HabboHotel.Rooms.PathFinding
{
    sealed class Vector3D
    {
        private int _x;
        private int _y;
        private double _z;

        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public double Z
        {
            get => _z;
            set => _z = value;
        }

        public Vector3D() { }

        public Vector3D(int x, int y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public Vector2D ToVector2D()
        {
            return new Vector2D(_x, _y);
        }
    }
}
