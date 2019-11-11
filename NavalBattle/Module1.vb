
Module Module1

    Sub Main()
        'Dim game As Game = New Game()
        'Game.Run()
        'Game.Dispose()
        Dim width As Integer = 10
        Dim height As Integer = 10
        Dim map As Map = New Map(width, height)
        map.PutShip(4, 4, Ship.Submarine, Orientation.Horizontal)
        map.PutShip(7, 6, Ship.Submarine, Orientation.Horizontal)
        map.PutShip(7, 7, Ship.Destroyer, Orientation.Horizontal)

        map.Attack(6, 6)
        map.Attack(8, 6)
        map.Attack(7, 5)


        map.Attack(4, 4)
        map.Attack(3, 4)
        map.Attack(4, 3)
        map.Attack(5, 4)
        map.Attack(4, 5)

        map.Attack(7, 7)
        map.Attack(8, 7)
        map.Attack(9, 7)

        map.Attack(6, 7)

        map.Attack(7, 6)
        map.Attack(8, 6)
        map.Attack(9, 6)

        map.Attack(7, 8)
        map.Attack(8, 8)
        map.Attack(9, 8)

        Dim mapView As HouseStatus() = map.GetEnemyVisionMap()
        For j As Integer = 0 To height - 1
            If j = 0 Then
                Console.Write("  ")
                For k As Integer = 0 To width - 1
                    Console.Write(k.ToString() + " ")
                Next
                Console.WriteLine()
            End If
            For i As Integer = 0 To width - 1
                If i = 0 Then
                    Console.Write(j.ToString() + " ")
                End If
                Select Case mapView(i + j * width)
                    Case HouseStatus.Normal
                        Console.Write("_ ")
                    Case HouseStatus.Hit
                        Console.Write("X ")
                    Case HouseStatus.Missed
                        Console.Write("O ")
                End Select
            Next
            Console.WriteLine()
        Next

        Dim cloats As Cloat() = CloatBacktracking.bk(mapView, width, height)

        For Each cloat As Cloat In cloats
            Console.WriteLine("Cloat: x: " + cloat.Position.x.ToString() + ", y:" + cloat.Position.y.ToString() + ", w:" + cloat.Size.width.ToString() + ", h:" + cloat.Size.height.ToString())
        Next

        Dim cloatPools As CloatPool() = CloatJoin.Join(cloats)
        For Each cloatPool In cloatPools
            cloatPool.Print()
            Console.WriteLine()
        Next

        Console.Read()
    End Sub

End Module
