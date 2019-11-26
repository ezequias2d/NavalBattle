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
    Private winAndLoseGameContext As GUIContext

    Private carrierNum As Integer = 1
    Private battleShipNum As Integer = 1
    Private destroyerNum As Integer = 2
    Private submarineNum As Integer = 1
    Private labelTexts As String() = New String() {"Porta Avioes", "Navio de Guerra", "Encouracado", "Submarino"}
    Private toPut As Ship
    Private orientation As Orientation

    Private label As Label

    Private player2IAMap As IAIMap
    Private player2IA As IAIPlayer

    Public Sub New(sizeX As Integer, sizeY As Integer)
        updates = New LinkedList(Of IUpdate)
        navalGame = New NavalGame(sizeX, sizeY, PlayerID.Player1)
        player2IA = New AI1Player(battleShipNum, carrierNum, destroyerNum, submarineNum)
        player2IAMap = New AI1Map()
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
        If navalGame.CurrentPlayer <> PlayerID.Player1 Then
            Return
        End If
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
            If carrierNum > 0 OrElse battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 Then
                GUIController.ChangeContext(putShipContext)
            Else
                toPut = Ship.None
                navalGame.Start()
            End If
            navalGame.FillMap(navalMap)
        Else
            navalGame.Attack(obj.IndexX, obj.IndexY)
            navalGame.FillMap(navalMap)
            If navalGame.CurrentPlayer <> PlayerID.Player1 Then
                GUIController.CurrentContext.MovableCursor = False
            End If
        End If
        UpdateLabelName()
    End Sub

    Private Sub Fire1Map(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If navalGame.CurrentPlayer <> PlayerID.Player1 Then
            Return
        End If

        If toPut <> Ship.None AndAlso (battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 OrElse carrierNum > 0) Then
            If orientation = Orientation.Horizontal Then
                orientation = Orientation.Vertical
            Else
                orientation = Orientation.Horizontal
            End If
        End If

    End Sub

    Private Sub Fire2Map(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        navalGame.ForceViewPlayer2Map = Not navalGame.ForceViewPlayer2Map
        navalGame.FillMap(navalMap)
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
            Dim button As Button = New Button(i, 0, i, New Vector2(0, (i - 1) * 32), labelTexts(i), Vector2.One)
            putShipContext.Add(button)
            button.OnFire0 = AddressOf Fire0PutShip
        Next

    End Sub

    Private Function CreateWinAndLoseLabel() As GUILabel

        Dim n As Integer = GUIController.CurrentContext.NextNegative()
        Dim finalText As GUILabel = New GUILabel(n, 0, n, Vector2.Zero, New Label("Label", Color.White, Label.Font))
        finalText.Scale = 2 * Vector2.One
        If navalGame.GetWin() = PlayerID.Player1 Then
            'Win
            finalText.Label.Text = "Win Game"
            finalText.Label.Color = Color.DarkGreen
        Else
            'Lose
            finalText.Label.Text = "Lose Game"
            finalText.Label.Color = Color.DarkRed
        End If
        Return finalText
    End Function

    Private Sub CreateAIStatusView()
        Dim position As Vector2 = Vector2.Zero
        position.X = Camera.InternalDimensions.X / 2.0F - 20.0F
        position.Y = Camera.InternalDimensions.Y / 2.0F - 16.0F

        Dim statusView As AIStatusViewer = New AIStatusViewer(sizeX + 1 + (sizeY - 1) * sizeY, sizeX, sizeY - 1, position, naval, player2IA)

        GUIController.CurrentContext.Add(statusView)
    End Sub

    Private Sub CreateLabel()
        label = New Label("Label", Color.White, Label.Font)
        Dim position As Vector2 = Vector2.Zero
        position.Y = Camera.InternalDimensions.Y / 2.0F - 8.0F
        label.Position = position
        label.DrawEnable = True
        Camera.Drawings.Add(label)
    End Sub

    Private Sub UncreateLabel()
        Camera.Drawings.Remove(label)
    End Sub

    Private Sub UpdateLabelName()
        Select Case navalGame.CurrentPlayer
            Case PlayerID.Player1
                label.Text = "Player 1"
            Case PlayerID.Player2
                label.Text = "Player 2"
        End Select
    End Sub

    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        CreatePutShipContext()
        CreateLabel()
        naval = content.Load(Of Texture2D)("naval")
        font = content.Load(Of SpriteFont)("fonts/PressStart2P")
        CreateAIStatusView()

        Dim area As Vector2 = New Vector2(Camera.InternalDimensions.X, Camera.InternalDimensions.Y - 32)
        navalMap = New NavalMap(GUIController.CurrentContext, naval, area, sizeX, sizeY, AddressOf Fire0Map, AddressOf Fire1Map, AddressOf Fire2Map, AddressOf CalculateColor)
        navalMap.Position = New Vector2(0, -16.0F)

        navalGame.FillMap(navalMap)

        GUIController.ChangeContext(putShipContext)

        If navalGame.CurrentPlayer <> PlayerID.Player2 Then
            'troca para player2
            navalGame.SwapPlayer()
        End If

        'Cria novo mapa com a IA
        Dim player2Map As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation)()
        player2Map = player2IAMap.GenereteMap(sizeX, sizeY, battleShipNum, carrierNum, destroyerNum, submarineNum)

        'Aplica mapa ao player2
        For Each s As (ship As Ship, position As (x As Integer, y As Integer), orientation As Orientation) In player2Map
            navalGame.PutShip(s.position.x, s.position.y, s.ship, s.orientation)
        Next

        'volta para player1
        navalGame.SwapPlayer()



    End Sub

    'Contador de jogo
    Dim count As Double = 0.0
    Dim selectedShot As Boolean = False
    Dim endGame As Boolean = False

    Public Overrides Sub Update(gameTime As GameTime)
        MyBase.Update(gameTime)

        If Not endGame Then
            If Not player2IA.IsInProcessing And Not player2IA.IsProcessingComplete() And Not selectedShot Then
                player2IA.StartAttackProcessing(navalGame.GetEnemyVisionMap(PlayerID.Player1), sizeX, sizeY)
            ElseIf navalGame.CurrentPlayer = PlayerID.Player2 And player2IA.IsProcessingComplete() Then
                Dim shoot As (x As Integer, y As Integer) = player2IA.NextResult()
                GUIController.CurrentContext.SelectObject(shoot.x, shoot.y)
                selectedShot = True
                navalGame.FillMap(navalMap)
            End If

            If selectedShot Then
                count += gameTime.ElapsedGameTime.TotalSeconds
                If count > 1.0 Then
                    Dim current As GUIObject = GUIController.CurrentContext.GetCurrent()
                    navalGame.Attack(current.IndexX, current.IndexY)
                    navalGame.FillMap(navalMap)
                    count = 0.0
                    selectedShot = False
                    GUIController.CurrentContext.MovableCursor = True
                End If
            End If
            UpdateLabelName()
            If navalGame.IsEnd() AndAlso navalGame.GetWin() <> PlayerID.Undefined Then

                Dim winAndLoseLabel As GUILabel = CreateWinAndLoseLabel()
                winAndLoseLabel.LayerDetph = 16
                Dim painelSize As Vector2 = winAndLoseLabel.Label.Measure(winAndLoseLabel.Scale) + 2 * Vector2.One
                Dim colorPainel As ColorPainel = New ColorPainel(GUIController.CurrentContext.NextNegative(), 0, 0, -painelSize / 2.0F, painelSize, Color.GhostWhite)
                colorPainel.LayerDetph = 15

                GUIController.CurrentContext.Add(winAndLoseLabel)
                GUIController.CurrentContext.Add(colorPainel)

                endGame = True
                GUIController.CurrentContext.CursorEnable = False
                navalMap.RemoveButtons()
            End If
        End If
    End Sub


    Public Overrides Sub UnloadContent()
        UncreateLabel()
        MyBase.UnloadContent()
    End Sub

End Class
