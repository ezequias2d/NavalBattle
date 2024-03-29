﻿Imports System.Text
Imports NavalBattle

Public Structure SubMap
    Implements IEquatable(Of SubMap)

    ' Peso
    Private _Weight As UInteger
    Private _shipsDetails As HashSet(Of (Ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger))

    Public Sub New(details As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)(), num As UInteger)
        _shipsDetails = New HashSet(Of (Ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger))(details)
        For i As Integer = 0 To num
            Select Case details(i).ship
                Case Ship.Battleship
                    Battleship += 1
                Case Ship.Carrier
                    Carrier += 1
                Case Ship.Destroyer
                    Destroyer += 1
                Case Ship.Submarine
                    Submarine += 1
            End Select
            _Weight += details(i).ship
        Next
    End Sub

    Private Sub New(details As ISet(Of (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)), weight As UInteger)
        _shipsDetails = details
        Dim count As UInteger = 0
        For Each detail As (Ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger) In details
            Select Case detail.Ship
                Case Ship.Battleship
                    Battleship += 1
                Case Ship.Carrier
                    Carrier += 1
                Case Ship.Destroyer
                    Destroyer += 1
                Case Ship.Submarine
                    Submarine += 1
            End Select
            count += 1
        Next
        _Weight = weight
    End Sub

    Public Function GetAvaliable(avaliables As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger)) As (Battleship As UInteger, Carrier As UInteger, Destroyer As UInteger, Submarine As UInteger, Weight As UInteger)
        Dim battleship As UInteger = avaliables.Battleship
        Dim carrier As UInteger = avaliables.Carrier
        Dim destroyer As UInteger = avaliables.Destroyer
        Dim submarine As UInteger = avaliables.Submarine

        For Each detail As (Ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger) In _shipsDetails
            If Not detail.complete Then
                Continue For
            End If

            Select Case detail.Ship
                Case Ship.Battleship
                    battleship -= 1
                Case Ship.Carrier
                    carrier -= 1
                Case Ship.Destroyer
                    destroyer -= 1
                Case Ship.Submarine
                    submarine -= 1
            End Select
        Next

        Return (battleship, carrier, destroyer, submarine, Weight)
    End Function

    Public Shared Function Unify(submaps As SubMap(), num As UInteger) As SubMap
        If num = 0 Then
            Return Nothing
        End If

        Dim details As ISet(Of (Ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger))
        details = New HashSet(Of (Ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger))(submaps(0)._shipsDetails)

        Dim weigth As UInteger = submaps(0).Weight
        For i As UInteger = 1 To num - 1
            Dim submap As SubMap = submaps(i)
            details.UnionWith(submap._shipsDetails)
            weigth += submap.Weight
        Next

        Return New SubMap(details, weigth)
    End Function

    Public ReadOnly Property Battleship As UInteger
    Public ReadOnly Property Carrier As UInteger
    Public ReadOnly Property Destroyer As UInteger
    Public ReadOnly Property Submarine As UInteger
    Public ReadOnly Property Weight As UInteger
        Get
            Return _Weight
        End Get
    End Property

    Public ReadOnly Property Details As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)()
        Get
            Return _shipsDetails.ToArray()
        End Get
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is SubMap Then
            Dim aux As SubMap = obj
            Return Equals(aux)
        End If
        Return False
    End Function

    Public Overloads Function Equals(other As SubMap) As Boolean Implements IEquatable(Of SubMap).Equals
        Return other._shipsDetails.SetEquals(_shipsDetails)
    End Function

    Public Shared Sub GenereteSubMap(ByRef output As SubMap(), map As HouseStatus(), width As Integer, height As Integer, cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger)
        output = GenereteSubMap(map, width, height, cloatPool, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine)
    End Sub

    Public Shared Function GenereteSubMap(map As HouseStatus(), width As Integer, height As Integer, cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger) As SubMap()
        Dim aiMaps As ICollection(Of SubMap) = New LinkedList(Of SubMap)()
        Dim a As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)()
        a = New(ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)(maxBattlleship + maxCarrier + maxDestroyer + maxSubmarine) {}
        GenereteAIMapInternal(map, width, height, a, 0, cloatPool, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine, aiMaps)

        Return aiMaps.ToArray()
    End Function

    Private Shared Sub GenereteAIMapInternal(map As HouseStatus(), width As Integer, height As Integer, ByRef a As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)(),
                                             k As UInteger, cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger, ByRef aiMaps As ICollection(Of SubMap))
        Dim cNum As UInteger = (maxBattlleship + maxCarrier + maxDestroyer + maxSubmarine) * width * height
        Dim c As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)() = New(ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)(cNum) {}

        If cloatPool.Count = 0 Then
            Dim subMap As SubMap = New SubMap(a, k)

            If Not aiMaps.Contains(subMap) Then
                aiMaps.Add(subMap)
            End If
        Else
            k += 1

            GenereteCandidates(map, width, height, cloatPool, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine, c, cNum)
            For i = 0 To cNum - 1
                a(k - 1) = c(i)
                Dim cloatPoolNext As CloatPool = New CloatPool(cloatPool)
                Dim m As HouseStatus() = CopyMap(map, width, height)

                cloatPoolNext.PutShip(c(i).position.x, c(i).position.y, c(i).ship, c(i).orientation)
                PutShip(c(i).position.x, c(i).position.y, c(i).ship, c(i).orientation, m, width, height)
                Select Case c(i).ship
                    Case Ship.Battleship
                        GenereteAIMapInternal(m, width, height, a, k, cloatPoolNext, maxBattlleship - 1, maxCarrier, maxDestroyer, maxSubmarine, aiMaps)
                    Case Ship.Carrier
                        GenereteAIMapInternal(m, width, height, a, k, cloatPoolNext, maxBattlleship, maxCarrier - 1, maxDestroyer, maxSubmarine, aiMaps)
                    Case Ship.Destroyer
                        GenereteAIMapInternal(m, width, height, a, k, cloatPoolNext, maxBattlleship, maxCarrier, maxDestroyer - 1, maxSubmarine, aiMaps)
                    Case Ship.Submarine
                        GenereteAIMapInternal(m, width, height, a, k, cloatPoolNext, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine - 1, aiMaps)
                End Select
            Next
        End If
    End Sub

    Private Shared Sub PutShip(x As Integer, y As Integer, ship As Ship, orientation As Orientation, ByRef map As HouseStatus(), width As Integer, height As Integer)
        If Not IsPuttable(x, y, ship, orientation, map, width, height) Then
            Return
        End If

        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

        For i As Integer = x To x + size.width - 1
            For j As Integer = y To y + size.height - 1
                map(i + j * width) = HouseStatus.Missed
            Next
        Next
    End Sub

    Private Shared Function CopyMap(map As HouseStatus(), width As Integer, height As Integer) As HouseStatus()
        Dim copy As HouseStatus() = New HouseStatus(width * height) {}
        For i As Integer = 0 To width - 1
            For j As Integer = 0 To height - 1
                copy(i + j * width) = map(i + j * width)
            Next
        Next
        Return copy
    End Function

    Private Shared Function IsPuttable(x As Integer, y As Integer, ship As Ship, orientation As Orientation, map As HouseStatus(), mapWidth As Integer, mapHeight As Integer)
        Dim output As Boolean = True
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)
        output = IsFreeArea(x, y, size.width, size.height, map, mapWidth, mapHeight)

        Return output
    End Function

    Private Shared Function IsComplete(x As Integer, y As Integer, ship As Ship, orientation As Orientation, map As HouseStatus(), mapWidth As Integer, mapHeight As Integer)
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)
        If x + size.width - 1 < mapWidth AndAlso y + size.height - 1 < mapHeight Then
            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    If Not (map(i + j * mapWidth) = HouseStatus.Hit) Then
                        Return False
                    End If
                Next
            Next
        Else
            Return False
        End If
        Return True
    End Function

    Private Shared Function GetCompleteCount(x As Integer, y As Integer, ship As Ship, orientation As Orientation, map As HouseStatus(), mapWidth As Integer, mapHeight As Integer) As UInteger
        Dim output As UInteger = 0
        Dim size As (width As Integer, height As Integer) = GetSize(ship, orientation)

        If x + size.width - 1 < mapWidth AndAlso y + size.height - 1 < mapHeight Then
            For i As Integer = x To x + size.width - 1
                For j As Integer = y To y + size.height - 1
                    If (map(i + j * mapWidth) = HouseStatus.Hit) Then
                        output += 1
                    End If
                Next
            Next
        End If
        Return output
    End Function

    Private Shared Function IsFreeArea(x As Integer, y As Integer, width As Integer, height As Integer, map As HouseStatus(), mapWidth As Integer, mapHeight As Integer) As Boolean
        Dim output As Boolean = True

        If x + width - 1 < mapWidth AndAlso y + height - 1 < mapHeight Then
            For i As Integer = x To x + width - 1
                For j As Integer = y To y + height - 1
                    output = output AndAlso (map(i + j * mapWidth) <> HouseStatus.Missed)
                    If Not output Then
                        Exit For
                    End If
                Next
                If Not output Then
                    Exit For
                End If
            Next
        Else
            output = False
        End If

        Return output
    End Function

    Private Shared Sub GenereteCandidates(map As HouseStatus(), width As Integer, height As Integer, cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger, ByRef c As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation, complete As Boolean, weight As UInteger)(), ByRef cNum As UInteger)
        Dim orientations As Orientation() = New Orientation() {Orientation.Horizontal, Orientation.Vertical}

        Dim j As Integer = 0
        Dim ships As Ship() = {Ship.Battleship, Ship.Carrier, Ship.Destroyer, Ship.Submarine}

        For Each orientation In orientations
            For Each ship As Ship In ships
                Dim flag As Boolean = False
                Select Case ship
                    Case Ship.Battleship
                        flag = (maxBattlleship > 0)
                    Case Ship.Carrier
                        flag = (maxCarrier > 0)
                    Case Ship.Destroyer
                        flag = (maxDestroyer > 0)
                    Case Ship.Submarine
                        flag = (maxSubmarine > 0)
                End Select

                If flag Then
                    Dim sizeShip As (x As Integer, y As Integer) = GetSize(ship, orientation)
                    For x As Integer = Math.Max(0, cloatPool.Position.x - sizeShip.x + 1) To Math.Min(width - sizeShip.x, cloatPool.Position.x + cloatPool.Size.width - 1)
                        For y As Integer = Math.Max(0, cloatPool.Position.y - sizeShip.y + 1) To Math.Min(height - sizeShip.y, cloatPool.Position.y + cloatPool.Size.height - 1)
                            If IsPuttable(x, y, ship, orientation, map, width, height) Then
                                If cloatPool.IsCollision(x, y, ship, orientation) AndAlso Not (ship = Ship.Submarine AndAlso orientation = Orientation.Vertical) Then
                                    c(j) = (ship, (x, y), orientation, IsComplete(x, y, ship, orientation, map, width, height), GetCompleteCount(x, y, ship, orientation, map, width, height))
                                    j += 1
                                End If
                            End If
                        Next
                    Next
                End If
            Next
        Next
        cNum = j + 1
    End Sub

    Public Overrides Function ToString() As String
        Dim stringBuilder As StringBuilder = New StringBuilder()
        stringBuilder.Append("Battleship: ")
        stringBuilder.AppendLine(Battleship.ToString())
        stringBuilder.Append("Carrier: ")
        stringBuilder.AppendLine(Carrier.ToString())
        stringBuilder.Append("Destroyer: ")
        stringBuilder.AppendLine(Destroyer.ToString())
        stringBuilder.Append("Submarine: ")
        stringBuilder.AppendLine(Submarine.ToString())
        stringBuilder.Append("Weight: ")
        stringBuilder.AppendLine(Weight.ToString())
        Return stringBuilder.ToString()
    End Function
End Structure
