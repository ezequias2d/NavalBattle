Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class NavalBattleScene
    Inherits GameScene

    Private naval As Texture2D

    Private sizeX As Integer
    Private sizeY As Integer

    Private navalMap As NavalMap
    Private font As SpriteFont

    Private navalGame As NavalGame

    Private putShipContext As GUIContext

    Private carrierNum As Integer = 1
    Private battleShipNum As Integer = 1
    Private destroyerNum As Integer = 2
    Private submarineNum As Integer = 1
    Private labelTexts As String() = New String() {"Porta Avioes", "Navio de Guerra", "Encouracado", "Submarino"}
    Private toPut As Ship
    Private orientation As Orientation

    'Modelos
    Private carrierModel As Sprite
    Private battleshipModel As Sprite
    Private destroyerModel As Sprite
    Private submarineModel As Sprite

    Public Sub New(sizeX As Integer, sizeY As Integer)
        updates = New LinkedList(Of IUpdate)
        navalGame = New NavalGame(sizeX, sizeY, PlayerID.Player1)
        Me.sizeX = sizeX
        Me.sizeY = sizeY
    End Sub

    Private Sub Fire0PutShip(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        toPut = Ship.None
        Select Case obj.IndexY
            Case 0
                'Porta avioes
                If carrierNum > 0 Then
                    toPut = Ship.Carrier
                End If
            Case 1
                'navio de guerra
                If battleShipNum > 0 Then
                    toPut = Ship.Battleship
                End If
            Case 2
                'encouraçado
                If destroyerNum > 0 Then
                    toPut = Ship.Destroyer
                End If
            Case 3
                'submarino
                If submarineNum > 0 Then
                    toPut = Ship.Submarine
                End If
            Case Else
                toPut = Ship.None
        End Select
        If toPut <> Ship.None Then
            GUIController.GoBack()

        End If
    End Sub

    Private Sub Fire0Map(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If toPut <> Ship.None AndAlso (battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 OrElse carrierNum > 0) Then
            If navalGame.IsPuttable(obj.IndexX, obj.IndexY, toPut, orientation) Then
                navalGame.PutShip(obj.IndexX, obj.IndexY, toPut, orientation)
                Select Case toPut
                    Case Ship.Carrier
                        carrierNum -= 1
                    Case Ship.Battleship
                        battleShipNum -= 1
                    Case Ship.Destroyer
                        destroyerNum -= 1
                    Case Ship.Submarine
                        submarineNum -= 1
                End Select
            End If
            Console.WriteLine(obj.IndexX.ToString() + ", " + obj.IndexY.ToString())
            If carrierNum > 0 OrElse battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 Then
                GUIController.ChangeContext(putShipContext)
            Else
                toPut = Ship.None
            End If
            navalGame.FillMap(navalMap)
        Else
            navalGame.Attack(obj.IndexX, obj.IndexY)
        End If
    End Sub

    Private Sub Fire1Map(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If toPut <> Ship.None AndAlso (battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 OrElse carrierNum > 0) Then
            If orientation = Orientation.Horizontal Then
                orientation = Orientation.Vertical
            Else
                orientation = Orientation.Horizontal
            End If
        End If
    End Sub

    Private Sub Fire2Map(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)

    End Sub

    Private Function CalculateColor(ByVal obj As GUIObject) As Color
        Dim color As Color
        If toPut <> Ship.None AndAlso (battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 OrElse carrierNum > 0) Then
            navalGame.FillMap(navalMap)

            If navalGame.IsPuttable(obj.IndexX, obj.IndexY, toPut, orientation) Then
                color = Color.LawnGreen
                Dim size As (width As Integer, height As Integer) = Map.GetSize(toPut, orientation)
                Dim piece As Integer = 0
                For i As Integer = obj.IndexX To obj.IndexX + size.width - 1
                    For j As Integer = obj.IndexY To obj.IndexY + size.height - 1
                        navalMap.SetHouse(i, j, toPut, orientation, piece)
                        Console.WriteLine(i.ToString() + "," + j.ToString())
                        piece += 1
                    Next
                Next
            Else
                color = Color.DarkRed
            End If
        Else
            If navalGame.IsAttackable(obj.IndexX, obj.IndexY) Then
                color = Color.LawnGreen
            Else
                color = Color.DarkRed
            End If
        End If
        Return color
    End Function

    Private Sub CreatePutShipContext()
        putShipContext = New GUIContext(GUIController.MainContext.Area)
        For i As Integer = 0 To 3
            Dim button As Button = New Button(i, 0, i, New Vector2(0, (i - 1) * 24), labelTexts(i), Vector2.One * 0.5F)
            putShipContext.Add(button)
            button.OnFire0 = AddressOf Fire0PutShip
        Next

    End Sub

    Private Sub LoadModels()
        carrierModel = New Sprite()
        battleshipModel = New Sprite()
        destroyerModel = New Sprite()
        submarineModel = New Sprite()

        carrierModel.Frame = New Frame(naval, New Rectangle())
    End Sub


    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        CreatePutShipContext()
        naval = content.Load(Of Texture2D)("naval")
        font = content.Load(Of SpriteFont)("fonts/PressStart2P")
        Dim area As Vector2 = New Vector2(Camera.InternalDimensions.X, Camera.InternalDimensions.Y - 16)
        navalMap = New NavalMap(GUIController.CurrentContext, naval, area, sizeX, sizeY, AddressOf Fire0Map, AddressOf Fire1Map, AddressOf Fire2Map, AddressOf CalculateColor)
        navalMap.Position = New Vector2(0, -8.0F)

        'For i As Integer = 0 To sizeX - 1
        'Next
        'navalGame.PutShip(0, 0, Ship.Destroyer, Orientation.Horizontal)
        navalGame.FillMap(navalMap)
        GUIController.ChangeContext(putShipContext)
    End Sub

End Class
