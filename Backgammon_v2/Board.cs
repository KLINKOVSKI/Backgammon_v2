using Backgammon_v2;
using System;

namespace Backgammon_v2
{
    public delegate (int cub1, int cub2) RollCubeDelegate();
    public struct SpikeData
    {
        public Spike Spike { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int l1 { get; set; }
        public int l2 { get; set; }
        public int l3 { get; set; }
    }

    public class Board
    {
        public Spike[,] Spikes { get; }
        public bool FirstClick { get; private set; }
        public SpikeData srcSpike { get; private set; }
        public bool BlackTurn { get; set; }
        public int Cube1 { get; set; }
        public int Cube2 { get; set; }
        public int eatenW { get; set; }
        public int eatenB { get; set; }
        public int blacksOut { get; set; }
        public int whitesOut { get; set; }


        public int numTurns;
        public static event RollCubeDelegate RollCubeEvent;
        public Board()
        {
            MainWindow.OnSpikeClicked += OnSpikeClicked;
            FirstClick = true;
            numTurns = -1;
            blacksOut = 0;
            whitesOut = 0;
            Spikes = new Spike[2, 12];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Spikes[i, j] = new Spike();
                }
            }

            for (int i = 0; i < 12; i++)
            {
                Spikes[0, i].SpikeId = 11 - i;
            }
            for (int i = 0; i < 12; i++)
            {
                Spikes[1, i].SpikeId = 12 + i;
            }

            Spikes[0, 0].SoldiersCount = 5;
            Spikes[0, 0].Black = true;
            Spikes[1, 0].SoldiersCount = 5;
            Spikes[1, 0].Black = false;

            Spikes[0, 4].SoldiersCount = 3;
            Spikes[0, 4].Black = false;
            Spikes[1, 4].SoldiersCount = 3;
            Spikes[1, 4].Black = true;

            Spikes[0, 4].SoldiersCount = 3;
            Spikes[0, 4].Black = false;
            Spikes[1, 4].SoldiersCount = 3;
            Spikes[1, 4].Black = true;

            Spikes[0, 6].SoldiersCount = 5;
            Spikes[0, 6].Black = false;
            Spikes[1, 6].SoldiersCount = 5;
            Spikes[1, 6].Black = true;

            Spikes[0, 11].SoldiersCount = 2;
            Spikes[0, 11].Black = true;
            Spikes[1, 11].SoldiersCount = 2;
            Spikes[1, 11].Black = false;

            Spikes[0, 0].SoldiersCount = 5;
            Spikes[0, 0].Black = true;
            Spikes[1, 0].SoldiersCount = 5;
            Spikes[1, 0].Black = false;

            Spikes[0, 4].SoldiersCount = 3;
            Spikes[0, 4].Black = false;
            Spikes[1, 4].SoldiersCount = 3;
            Spikes[1, 4].Black = true;

            Spikes[0, 4].SoldiersCount = 3;
            Spikes[0, 4].Black = false;
            Spikes[1, 4].SoldiersCount = 3;
            Spikes[1, 4].Black = true;


            Spikes[0, 11].SoldiersCount = 2;
            Spikes[0, 11].Black = true;
            Spikes[1, 11].SoldiersCount = 2;
            Spikes[1, 11].Black = false;
        }

        private void OnSpikeClicked(int row, int column, int cube1, int cube2)
        {
            if (FirstClick)
            {
                if (eatenB <= 0 && eatenW <= 0)
                    if (Spikes[row, column].IsEmpty() || Spikes[row, column].Black != BlackTurn) { return; }
                    else if (Spikes[row, column].Black != BlackTurn) { return; }

                if (numTurns == -1)
                {
                    Cube1 = cube1;
                    Cube2 = cube2;
                    SetNumTurns(row, column);
                }

                if (Spikes[row, column].OutMode)
                {
                    Spikes[row, column].SoldiersCount--;
                    ClearAll();
                    if (row == 1) blacksOut++;
                    if (row == 0) whitesOut++;
                    numTurns--;
                    if (cube1 != cube2)
                    {
                        if (!BlackTurn)
                        {
                            int fartherSpike = IdFartherWhitesSpike();
                            if (cube1 > cube2 && cube1 > fartherSpike)
                            {
                                cube1 = fartherSpike;
                            }
                            if (cube2 > cube1 && cube2 > fartherSpike)
                            {
                                cube2 = fartherSpike;
                            }
                            if (Spikes[row, column].SpikeId == cube1 - 1 && row == 0) Cube1 = 0;
                            if (Spikes[row, column].SpikeId == cube2 - 1 && row == 0) Cube2 = 0;
                        }
                        else
                        {
                            int fartherSpike = IdFartherBlacksSpike();
                            if (cube1 > cube2 && cube1 > fartherSpike)
                            {
                                cube1 = fartherSpike;
                            }
                            if (cube2 > cube1 && cube2 > fartherSpike)
                            {
                                cube2 = fartherSpike;
                            }
                            if (Spikes[row, column].SpikeId == 24 - cube1 && row == 1) Cube1 = 0;
                            if (Spikes[row, column].SpikeId == 24 - cube2 && row == 1) Cube2 = 0;
                        }
                    }

                    if (WhitesInHouse())
                    {
                        ShowWhitesOut(Cube1, Cube2);
                    }
                    if (BlacksInHouse())
                    {
                        ShowBlacksOut(Cube1, Cube2);
                    }

                    HandleEndTurn();
                    FirstClick = true;
                    return;
                }
                if (eatenB > 0 && BlackTurn)
                {
                    if (Spikes[row, column].PreviewMode)
                    {
                        if (Spikes[row, column].SoldiersCount == 1 && Spikes[row, column].Black == !BlackTurn)
                        {
                            Spikes[row, column].SoldiersCount--;
                            eatenW++;
                        }
                        if (Spikes[row, column].IsEmpty()) Spikes[row, column].Black = true;
                        Spikes[row, column].SoldiersCount++;
                        ClearAll();
                        eatenB--;
                        numTurns--;
                        if (Spikes[row, column].SpikeId == cube1 - 1) Cube1 = 0;
                        if (Spikes[row, column].SpikeId == cube2 - 1) Cube2 = 0;
                        HandleEat(Cube1, Cube2);
                        HandleEndTurn();
                        FirstClick = true;
                        return;
                    }
                }

                if (eatenW > 0 && !BlackTurn)
                {
                    if (Spikes[row, column].PreviewMode)
                    {
                        if (Spikes[row, column].SoldiersCount == 1 && Spikes[row, column].Black == !BlackTurn)
                        {
                            Spikes[row, column].SoldiersCount--;
                            eatenB++;
                        }
                        if (Spikes[row, column].IsEmpty()) Spikes[row, column].Black = false;
                        Spikes[row, column].SoldiersCount++;
                        ClearAll();
                        eatenW--;
                        numTurns--;
                        if (Spikes[row, column].SpikeId == 24 - cube1) Cube1 = 0;
                        if (Spikes[row, column].SpikeId == 24 - cube2) Cube2 = 0;
                        HandleEat(Cube1, Cube2);
                        HandleEndTurn();
                        FirstClick = true;
                        return;
                    }
                }

                if (cube1 == cube2)
                {
                    int[] locs = GetDoublePreviewLocations(row, column, cube1);

                    srcSpike = new SpikeData
                    {
                        Column = column,
                        Row = row,
                        Spike = Spikes[row, column]
                    };

                    for (var i = 0; i < locs.Length; i++)
                    {
                        (int r, int c) = GetSpikeById(locs[i]);
                        if (r == -1 || c == -1) continue;
                        bool x = CheckSpike(r, c) && CheckMoveDirection(r, c);
                        if (!x) break;
                        Spikes[r, c].PreviewMode = true;
                    }

                    Spikes[row, column].Marked = true;
                    FirstClick = false;
                    return;
                }

                (int l1, int l2, int l3) = GetPreviewLocation(row, column, Cube1, Cube2);

                srcSpike = new SpikeData
                {
                    Column = column,
                    Row = row,
                    Spike = Spikes[row, column],
                    l1 = l1,
                    l2 = l2,
                    l3 = l3
                };

                if (l1 >= 0 && l1 < 24)
                {
                    (int rowl1, int columnl1) = GetSpikeById(l1);
                    Spikes[rowl1, columnl1].PreviewMode = CheckSpike(rowl1, columnl1) && CheckMoveDirection(rowl1, columnl1);
                }

                if (l2 >= 0 && l2 < 24)
                {
                    (int rowl2, int columnl2) = GetSpikeById(l2);
                    Spikes[rowl2, columnl2].PreviewMode = CheckSpike(rowl2, columnl2) && CheckMoveDirection(rowl2, columnl2);
                }

                if (l3 >= 0 && l3 < 24)
                {
                    (int rowl3, int columnl3) = GetSpikeById(l3);
                    Spikes[rowl3, columnl3].PreviewMode = CheckSpike(rowl3, columnl3) && CheckMoveDirection(rowl3, columnl3);
                }

                Spikes[row, column].Marked = true;
                FirstClick = false;
                return;
            }

            if (!Spikes[row, column].PreviewMode && !Spikes[row, column].Marked) return;

            Spikes[srcSpike.Row, srcSpike.Column].Marked = false;

            if (Spikes[row, column].IsEmpty())
            {
                Spikes[row, column].Black = srcSpike.Spike.Black;
            }

            if (Spikes[row, column].PreviewMode)
            {
                if (Spikes[row, column].SoldiersCount == 1 && srcSpike.Spike.Black != Spikes[row, column].Black)
                {
                    Spikes[row, column].SoldiersCount--;

                    if (Spikes[row, column].Black) eatenB++;
                    if (!Spikes[row, column].Black) eatenW++;

                    Spikes[row, column].Black = srcSpike.Spike.Black;
                }
                Spikes[srcSpike.Row, srcSpike.Column].SoldiersCount--;
                Spikes[row, column].SoldiersCount++;
            }

            if (cube1 != cube2)
            {
                if (Spikes[row, column].SpikeId == srcSpike.l1)
                {
                    Cube1 = 0;
                    numTurns--;
                }

                if (Spikes[row, column].SpikeId == srcSpike.l2)
                {
                    Cube2 = 0;
                    numTurns--;
                }

                if (Spikes[row, column].SpikeId == srcSpike.l3)
                {
                    Cube1 = 0;
                    Cube2 = 0;
                    numTurns -= 2;
                }
            }
            else
            {
                int turns = Math.Abs(srcSpike.Spike.SpikeId - Spikes[row, column].SpikeId) / cube1;
                numTurns -= turns;
            }
            ClearAll();
            if (WhitesInHouse())
            {
                ShowWhitesOut(Cube1, Cube2);
            }
            if (BlacksInHouse())
            {
                ShowBlacksOut(Cube1, Cube2);
            }
            HandleEndTurn();
            FirstClick = true;
        }

        private void HandleEndTurn()
        {
            if (numTurns <= 0)
            {
                numTurns = -1;
                BlackTurn = !BlackTurn;
                ClearAll();
                var ret = RollCubeEvent?.Invoke();
                if (ret == null)
                {
                    return;
                }

                for (int i = 0; i < (BlackTurn ? eatenB : eatenW); i++)
                {
                    if (!HandleEat(ret.Value.cub1, ret.Value.cub2))
                    {
                        BlackTurn = !BlackTurn;
                        RollCubeEvent?.Invoke();
                        break;
                    }
                }

                if (WhitesInHouse())
                {
                    ShowWhitesOut(ret.Value.cub1, ret.Value.cub2);
                }

                if (BlacksInHouse())
                {
                    ShowBlacksOut(ret.Value.cub1, ret.Value.cub2);
                }
            }
        }

        private void ClearAll()
        {
            for (int i = 0; i < Spikes.GetLength(0); i++)
            {
                for (int j = 0; j < Spikes.GetLength(1); j++)
                {
                    Spikes[i, j].PreviewMode = false;
                    Spikes[i, j].OutMode = false;
                    Spikes[i, j].Marked = false;
                }
            }
        }

        public Spike this[int row, int column] => Spikes[row, column];

        private bool CheckSpike(int row, int column)
        {
            return Spikes[row, column].Black == srcSpike.Spike.Black ||
                   Spikes[row, column].IsEmpty() ||
                   Spikes[row, column].SoldiersCount <= 1;
        }

        private bool CheckMoveDirection(int row, int column)
        {
            return Spikes[row, column].SpikeId <= srcSpike.Spike.SpikeId && !srcSpike.Spike.Black ||
                   Spikes[row, column].SpikeId >= srcSpike.Spike.SpikeId && srcSpike.Spike.Black;
        }

        private (int l1, int l2, int l3) GetPreviewLocation(int row, int column, int cube1, int cube2)
        {
            if (Spikes[row, column].Black)
            {
                int id1 = cube1 == 0 ? -1 : Spikes[row, column].SpikeId + cube1;
                int id2 = cube2 == 0 ? -1 : Spikes[row, column].SpikeId + cube2;
                int id3 = Spikes[row, column].SpikeId + cube1 + cube2;
                return (id1, id2, id3);
            }

            if (!Spikes[row, column].Black)
            {
                int id1 = cube1 == 0 ? -1 : Spikes[row, column].SpikeId - cube1;
                int id2 = cube2 == 0 ? -1 : Spikes[row, column].SpikeId - cube2;
                int id3 = Spikes[row, column].SpikeId - (cube1 + cube2);
                return (id1, id2, id3);
            }

            return (0, 0, 0);
        }

        private (int row, int column) GetSpikeById(int spikeId)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (Spikes[i, j].SpikeId == spikeId)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        private void SetNumTurns(int row, int column)
        {
            if (Cube1 == Cube2)
            {
                numTurns = 4;
                return;
            }

            numTurns = 0;
            (int l1, int l2, int l3) = GetPreviewLocation(row, column, Cube1, Cube2);

            if (l1 >= 0 && l1 < 24)
            {
                numTurns++;
            }

            if (l2 >= 0 && l2 < 24)
            {
                numTurns++;
            }

            if (l3 >= 0 && l3 < 24)
            {
                numTurns = numTurns < 2 ? numTurns + 1 : numTurns;
            }

            if (numTurns != 2 && WhitesInHouse())
            {
                for (int i = 1; i < 7; i++)
                {
                    if (Cube1 == i || Cube2 == i)
                    {
                        (int r, int c) = GetSpikeById(i - 1);
                        if (Spikes[r, c].Black == BlackTurn && !Spikes[r, c].IsEmpty())
                        {
                            numTurns++;
                        }
                    }
                }
            }
            if (numTurns != 2 && BlacksInHouse())
            {
                for (int i = 1; i < 7; i++)
                {
                    if (Cube1 == i || Cube2 == i)
                    {
                        (int r, int c) = GetSpikeById(24 - i);
                        if (Spikes[r, c].Black == BlackTurn && !Spikes[r, c].IsEmpty())
                        {
                            numTurns++;
                        }
                    }
                }
            }
            numTurns = numTurns > 2 ? 2 : numTurns;
        }

        private int[] GetDoublePreviewLocations(int row, int column, int cube)
        {
            int[] locs = new int[numTurns];
            for (int i = 1; i < numTurns + 1; i++)
            {
                locs[i - 1] = Spikes[row, column].SpikeId + (cube * i) * (Spikes[row, column].Black ? 1 : -1);
            }

            return locs;
        }

        private bool HandleEat(int cube1, int cube2)
        {
            int count = 0;
            if (eatenB > 0 && BlackTurn)
            {
                for (int i = 1; i < 7; i++)
                {
                    if (cube1 == i || cube2 == i)
                    {
                        (int r, int c) = GetSpikeById(i - 1);

                        bool previewMode = Spikes[r, c].Black == BlackTurn ||
                                          Spikes[r, c].IsEmpty() ||
                                          Spikes[r, c].SoldiersCount <= 1;
                        if (previewMode)
                            count++;

                        Spikes[r, c].PreviewMode = previewMode;
                    }
                }
            }
            if (eatenW > 0 && !BlackTurn)
            {
                for (int i = 1; i < 7; i++)
                {
                    if (cube1 == i || cube2 == i)
                    {
                        (int r, int c) = GetSpikeById(24 - i);
                        bool previewMode = Spikes[r, c].Black == BlackTurn ||
                                           Spikes[r, c].IsEmpty() ||
                                           Spikes[r, c].SoldiersCount <= 1;
                        if (previewMode)
                            count++;

                        Spikes[r, c].PreviewMode = previewMode;
                    }
                }
            }

            return count > 0;
        }

        private bool WhitesInHouse()
        {
            int count = whitesOut;
            for (int i = 6; i < 12; i++)
            {
                count += Spikes[0, i].SoldiersCount;
            }

            return count == 15;
        }
        private bool BlacksInHouse()
        {
            int count = blacksOut;
            for (int i = 6; i < 12; i++)
            {
                count += Spikes[1, i].SoldiersCount;
            }

            return count == 15;
        }

        private void ShowWhitesOut(int cube1, int cube2)
        {
            int fartherSpike = IdFartherWhitesSpike();
            if (cube1 > cube2 && cube1 > fartherSpike)
            {
                cube1 = fartherSpike;
            }
            if (cube2 > cube1 && cube2 > fartherSpike)
            {
                cube2 = fartherSpike;
            }

            for (int i = 1; i < 7; i++)
            {
                if (cube1 == i || cube2 == i)
                {
                    (int r, int c) = GetSpikeById(i - 1);
                    Spikes[r, c].OutMode = Spikes[r, c].Black == BlackTurn && !Spikes[r, c].IsEmpty();
                }
            }
        }
        private void ShowBlacksOut(int cube1, int cube2)
        {
            int fartherSpike = IdFartherBlacksSpike();
            if (cube1 >= cube2 && cube1 > fartherSpike)
            {
                cube1 = fartherSpike;
            }
            if (cube2 >= cube1 && cube2 > fartherSpike)
            {
                cube2 = fartherSpike;
            }
            for (int i = 1; i < 7; i++)
            {
                if (cube1 == i || cube2 == i)
                {
                    (int r, int c) = GetSpikeById(24 - i);
                    Spikes[r, c].OutMode = Spikes[r, c].Black == BlackTurn && !Spikes[r, c].IsEmpty();
                }
            }
        }

        private int IdFartherWhitesSpike()
        {
            for (int i = 6; i < 12; i++)
            {
                if (Spikes[0, i].SoldiersCount > 0)
                {
                    return 12 - i;
                }
            }

            return -1;
        }
        private int IdFartherBlacksSpike()
        {
            for (int i = 6; i < 12; i++)
            {
                if (Spikes[1, i].SoldiersCount > 0)
                {
                    return 12 - i;
                }
            }

            return -1;
        }
    }
}