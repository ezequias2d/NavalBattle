Imports NavalBattle

Public Class AI1Map
    Implements IAIMap

    Public Function GenereteMap(width As UInteger, height As UInteger, battleship As UInteger, carrier As UInteger, destroyer As UInteger, submarine As UInteger) As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)() Implements IAIMap.GenereteMap
        Dim output As ICollection(Of (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)) = New List(Of (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation))

        Dim chanceMap As ChanceMap = New ChanceMap(New HouseStatus(width * height) {}, width, height)

        While (battleship + carrier + destroyer + submarine > 0)
            Dim avaliable As (UInteger, UInteger, UInteger, UInteger, UInteger) = (battleship, carrier, destroyer, submarine, 1UL)

            chanceMap.CleanMap()
            chanceMap.AddMap(New(UInteger, UInteger, UInteger, UInteger, UInteger)() {avaliable})

            chanceMap.Adjuster()
            chanceMap.Invert()
            chanceMap.AdjusterInvert()

            Dim orientation As Orientation = Picker.ToRaffle(Of Orientation)(Orientation.Horizontal, Orientation.Vertical)
            Dim position As (x As Integer, y As Integer)
            If battleship > 0 Then
                position = Picker.ToRaffle(chanceMap, Ship.Battleship)
                If chanceMap.PutShip(position.x, position.y, Ship.Battleship, orientation) Then
                    battleship -= 1
                    output.Add((Ship.Battleship, position, orientation))
                End If
            ElseIf carrier > 0 Then
                position = Picker.ToRaffle(chanceMap, Ship.Carrier)
                If chanceMap.PutShip(position.x, position.y, Ship.Carrier, orientation) Then
                    carrier -= 1
                    output.Add((Ship.Carrier, position, orientation))
                End If
            ElseIf destroyer > 0 Then
                position = Picker.ToRaffle(chanceMap, Ship.Destroyer)
                If chanceMap.PutShip(position.x, position.y, Ship.Destroyer, orientation) Then
                    destroyer -= 1
                    output.Add((Ship.Destroyer, position, orientation))
                End If
            ElseIf submarine > 0 Then
                position = Picker.ToRaffle(chanceMap, Ship.Submarine)
                If chanceMap.PutShip(position.x, position.y, Ship.Submarine, orientation) Then
                    submarine -= 1
                    output.Add((Ship.Submarine, position, orientation))
                End If
            End If
        End While

        Return output.ToArray()
    End Function
End Class
