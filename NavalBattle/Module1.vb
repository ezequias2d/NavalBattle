
Module Module1

    Sub Main()
        Dim game As Game = New Game()
        game.Run()
        Game.Dispose()
        Dim width As Integer = 6
        Dim height As Integer = 6
        Dim map As Map = New Map(width, height)

        Dim aiMap As IAIMap = New AI1Map()
        Dim mapShips As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)()


        mapShips = aiMap.GenereteMap(width, height, 1, 1, 2, 1)

        For Each mapShip As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation) In mapShips
            map.PutShip(mapShip.position.x, mapShip.position.y, mapShip.ship, mapShip.orientation)
        Next




        'map.PutShip(4, 4, Ship.Submarine, Orientation.Horizontal)
        'map.PutShip(7, 6, Ship.Submarine, Orientation.Horizontal)
        'map.PutShip(7, 7, Ship.Destroyer, Orientation.Horizontal)
        'map.Attack(4, 4)

        'map.Attack(7, 6)
        'map.Attack(7, 7)

        map.PrintMap()

        Dim play As IAIPlayer = New AI1Player(1, 1, 2, 4)
        Console.WriteLine(play.GetAttack(map.GetEnemyVisionMap(), width, height))
        Console.WriteLine(play.GetAttack(map.GetEnemyVisionMap(), width, height))
        Console.WriteLine(play.GetAttack(map.GetEnemyVisionMap(), width, height))
        Console.WriteLine(play.GetAttack(map.GetEnemyVisionMap(), width, height))

        Console.ReadKey()

    End Sub

End Module
