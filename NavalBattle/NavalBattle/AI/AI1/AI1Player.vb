Imports NavalBattle

Public Class AI1Player
    Implements IAIPlayer

    Dim pieces As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger)

    Public Sub New(battleship As UInteger, carrier As UInteger, destroyer As UInteger, submarine As UInteger)
        pieces.Battleship = battleship
        pieces.Carrier = carrier
        pieces.Destroyer = destroyer
        pieces.Submarine = submarine
    End Sub

    Public Function GetAttack(map As HouseStatus(), width As Integer, height As Integer) As (x As Integer, y As Integer) Implements IAIPlayer.GetAttack
        Dim possibles As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)() = Resolver.CalculatePossibleAvailableParts(map, width, height, pieces)

        Dim possibleAvaliables As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)()
        possibleAvaliables = Resolver.CalculatePossibleAvailableParts(map, width, height, pieces)

        For Each possible In possibleAvaliables
            Console.WriteLine(possible)
        Next

        Dim chanceMap As ChanceMap = New ChanceMap(map, width, height)

        chanceMap.CleanMap()
        chanceMap.AddMap(possibleAvaliables)
        chanceMap.Adjuster()

        Return Picker.ToRaffle(chanceMap)
    End Function
End Class
