Imports System.Text

Public Structure SubMap
    Private _Weight As UInteger

    Public Sub New(ships As Ship(), num As UInteger)
        Me.New()
        For i As UInteger = 0 To num - 1
            Dim s As Ship = ships(i)
            Select Case s
                Case Ship.Battleship
                    Battleship += 1
                Case Ship.Carrier
                    Carrier += 1
                Case Ship.Destroyer
                    Destroyer += 1
                Case Ship.Submarine
                    Submarine += 1
            End Select
        Next
        _Weight = 1
    End Sub

    Public Sub New(battleship As UInteger, carrier As UInteger, destroyer As UInteger, submarine As UInteger)
        Me.New()
        Me.Battleship = battleship
        Me.Carrier = carrier
        Me.Destroyer = destroyer
        Me.Submarine = submarine
        Me._Weight = 1
    End Sub

    Public ReadOnly Property Battleship As UInteger
    Public ReadOnly Property Carrier As UInteger
    Public ReadOnly Property Destroyer As UInteger
    Public ReadOnly Property Submarine As UInteger
    Public ReadOnly Property Weight As UInteger
        Get
            Return _Weight
        End Get
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is SubMap Then
            Dim aux As SubMap = obj
            Return Battleship = aux.Battleship AndAlso
                Carrier = aux.Carrier AndAlso
                Destroyer = aux.Destroyer AndAlso
                Submarine = aux.Submarine
        End If
        Return False
    End Function

    Public Shared Function GenereteSubMap(cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger) As SubMap()
        Dim aiMaps As ICollection(Of SubMap) = New List(Of SubMap)
        Dim a As Ship() = New Ship(maxBattlleship + maxCarrier + maxDestroyer + maxSubmarine) {}
        GenereteAIMapInternal(a, 0, cloatPool, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine, aiMaps)

        Return aiMaps.ToArray()
    End Function

    Private Shared Sub GenereteAIMapInternal(ByRef a As Ship(), k As UInteger, cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger, ByRef aiMaps As ICollection(Of SubMap))
        Dim cNum As UInteger = (maxBattlleship + maxCarrier + maxDestroyer + maxSubmarine) * cloatPool.Size.width * cloatPool.Size.height
        Dim c As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)() = New(ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)(cNum) {}

        If cloatPool.Count = 0 Then
            Dim map As SubMap = New SubMap(a, k)
            If Not aiMaps.Contains(map) Then
                aiMaps.Add(New SubMap(a, k))
            Else
                For Each mapAux In aiMaps
                    If map.Equals(mapAux) Then
                        map._Weight += mapAux.Weight
                        aiMaps.Remove(mapAux)
                        aiMaps.Add(map)
                        Exit For
                    End If
                Next
            End If
        Else
            k += 1

            GenereteCandidates(cloatPool, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine, c, cNum)
            For i = 0 To cNum - 1
                a(k - 1) = c(i).ship
                Dim cloatPoolNext As CloatPool = New CloatPool(cloatPool)
                cloatPoolNext.PutShip(c(i).position.x, c(i).position.y, c(i).ship, c(i).orientation)
                Select Case c(i).ship
                    Case Ship.Battleship
                        GenereteAIMapInternal(a, k, cloatPoolNext, maxBattlleship - 1, maxCarrier, maxDestroyer, maxSubmarine, aiMaps)
                    Case Ship.Carrier
                        GenereteAIMapInternal(a, k, cloatPoolNext, maxBattlleship, maxCarrier - 1, maxDestroyer, maxSubmarine, aiMaps)
                    Case Ship.Destroyer
                        GenereteAIMapInternal(a, k, cloatPoolNext, maxBattlleship, maxCarrier, maxDestroyer - 1, maxSubmarine, aiMaps)
                    Case Ship.Submarine
                        GenereteAIMapInternal(a, k, cloatPoolNext, maxBattlleship, maxCarrier, maxDestroyer, maxSubmarine - 1, aiMaps)
                End Select
            Next
        End If
    End Sub

    Private Shared Sub GenereteCandidates(cloatPool As CloatPool, maxBattlleship As UInteger, maxCarrier As UInteger, maxDestroyer As UInteger, maxSubmarine As UInteger, ByRef c As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)(), ByRef cNum As UInteger)
        Dim orientations As Orientation() = New Orientation() {Orientation.Horizontal, Orientation.Vertical}

        Dim j As Integer = 0
        For Each orientation In orientations
            For x As Integer = 0 To cloatPool.Size.width - 1
                For y As Integer = 0 To cloatPool.Size.height - 1

                    If cloatPool.Item(x, y) Then
                        If maxBattlleship > 0 AndAlso cloatPool.IsPuttable(x, y, Ship.Battleship, orientation) Then
                            c(j) = (Ship.Battleship, (x, y), orientation)
                            j += 1
                        End If

                        If maxCarrier > 0 AndAlso cloatPool.IsPuttable(x, y, Ship.Carrier, orientation) Then
                            c(j) = (Ship.Carrier, (x, y), orientation)
                            j += 1
                        End If

                        If maxDestroyer > 0 AndAlso cloatPool.IsPuttable(x, y, Ship.Destroyer, orientation) Then
                            c(j) = (Ship.Destroyer, (x, y), orientation)
                            j += 1
                        End If

                        If maxSubmarine > 0 AndAlso cloatPool.IsPuttable(x, y, Ship.Submarine, orientation) Then
                            c(j) = (Ship.Submarine, (x, y), orientation)
                            j += 1
                        End If
                    End If
                Next
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
