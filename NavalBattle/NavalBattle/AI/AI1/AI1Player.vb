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

    Private Function ExistNotComplete(submaps As SubMap()) As Boolean
        Dim output As Boolean = False
        For Each submap In submaps
            For Each detail In submap.Details
                output = output OrElse (detail.ship <> Ship.None AndAlso Not detail.complete)
                If output Then
                    Exit For
                End If
            Next
            If output Then
                Exit For
            End If
        Next
        Return output
    End Function

    Public Function GetAttack(map As HouseStatus(), width As Integer, height As Integer) As (x As Integer, y As Integer) Implements IAIPlayer.GetAttack
        Dim possibleMaps As SubMap()
        possibleMaps = Resolver.CalculatePossibleAvailableParts(map, width, height, pieces)

        Dim chanceMap As ChanceMap = New ChanceMap(map, width, height)

        If ExistNotComplete(possibleMaps) Then
            For Each submap As SubMap In possibleMaps
                For Each detail In submap.Details
                    If Not detail.complete Then
                        chanceMap.ExplicitlyAdd(detail, submap.Weight)
                    End If
                Next
            Next
            chanceMap.Adjuster()
            Return chanceMap.GetMaxHouse()
        Else
            If possibleMaps.Count > 0 Then
                For Each submap As SubMap In possibleMaps
                    chanceMap.AddMap(submap.GetAvaliable(pieces))
                Next
            Else
                chanceMap.AddMap((pieces.Battleship, pieces.Carrier, pieces.Destroyer, pieces.Submarine, 1))
            End If
            chanceMap.Adjuster()
            Return Picker.ToRaffle(chanceMap)
        End If

        'Dim chanceMap As ChanceMap = New ChanceMap(map, width, height)
        'chanceMap.CleanMap()
        'If possibleMaps.Count > 0 Then
        '    For Each submap As SubMap In possibleMaps
        '        chanceMap.AddMap(submap.GetAvaliable(pieces))
        '        For Each detail In submap.Details
        '            If Not detail.complete Then
        '                chanceMap.ExplicitlyAdd(detail, submap.Weight)
        '            End If
        '        Next
        '    Next
        'Else
        '    chanceMap.AddMap((pieces.Battleship, pieces.Carrier, pieces.Destroyer, pieces.Submarine, 1))
        'End If

        'Return Picker.ToRaffle(chanceMap)
        'Return chanceMap.GetMaxHouse()
    End Function
End Class
