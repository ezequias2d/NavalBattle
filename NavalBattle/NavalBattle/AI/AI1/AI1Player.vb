Imports System.Threading
Imports NavalBattle

Public Class AI1Player
    Implements IAIPlayer

    Dim processThreading As Thread

    Dim pieces As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger)

    Private map As HouseStatus()
    Private width As Integer
    Private height As Integer
    Private result As (Integer, Integer)
    Private complete As Boolean

    Public Sub New(battleship As UInteger, carrier As UInteger, destroyer As UInteger, submarine As UInteger)
        pieces.Battleship = battleship
        pieces.Carrier = carrier
        pieces.Destroyer = destroyer
        pieces.Submarine = submarine
        complete = False
    End Sub

    Private Function ExistNotComplete(submaps As SubMap()) As Boolean
        For Each submap In submaps
            For Each detail In submap.Details
                If detail.ship <> Ship.None AndAlso Not detail.complete Then
                    Return True
                End If
            Next
        Next
        Return False
    End Function

    Private Sub ThreadTask()
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
            result = chanceMap.GetMaxHouse()
        Else
            If possibleMaps.Count > 0 Then
                For Each submap As SubMap In possibleMaps
                    chanceMap.AddMap(submap.GetAvaliable(pieces))
                Next
            Else
                chanceMap.AddMap((pieces.Battleship, pieces.Carrier, pieces.Destroyer, pieces.Submarine, 1))
            End If
            chanceMap.Adjuster()
            result = Picker.ToRaffle(chanceMap)
        End If
        complete = True
    End Sub

    Public Sub StartAttackProcessing(map As HouseStatus(), width As Integer, height As Integer) Implements IAIPlayer.StartAttackProcessing
        Me.map = map
        Me.width = width
        Me.height = height
        processThreading = New Thread(AddressOf ThreadTask)
        processThreading.Start()
    End Sub

    Public Function IsProcessingComplete() As Boolean Implements IAIPlayer.IsProcessingComplete
        Return complete
    End Function

    Public Function NextResult() As (x As Integer, y As Integer) Implements IAIPlayer.NextResult
        complete = False
        Return result
    End Function

    Public Function IsInProcessing() As Boolean Implements IAIPlayer.IsInProcessing
        Return processThreading IsNot Nothing AndAlso processThreading.ThreadState = ThreadState.Running
    End Function
End Class
