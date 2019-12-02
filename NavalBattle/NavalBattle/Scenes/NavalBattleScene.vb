Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Audio
Imports Microsoft.Xna.Framework.Graphics

Public Class NavalBattleScene
    Inherits GameScene

    Private naval As Texture2D
    Private naval2 As Texture2D

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
    Private labelTexts As String()
    Private toPut As Ship
    Private orientation As Orientation

    Private playerLabel As GUILabel

    Private player2IAMap As IAIMap
    Private player2IA As IAIPlayer
    Private menu As MenuScene

    Private winAndLoseContext As GUIContext

    Private controlsView As ControlsViewer
    Private labelMove As Label
    Private labelMoveMenu As Label
    Private labelFire0 As Label
    Private labelFire1 As Label
    Private labelFire2 As Label

    Private chanceMapViewer As ChanceMapViewer

    Private resource As Resources.ResourceManager

    Private shoot As SoundEffect
    Public Sub SetLanguage()
        If Language IsNot Nothing Then
            Select Case (Language)
                Case "pt-BR"
                    resource = My.Resources.ptBR.ResourceManager
                Case "en-EN"
                    resource = My.Resources.enEN.ResourceManager
                Case "es-ES"
                    resource = My.Resources.esES.ResourceManager
                Case "fr-FR"
                    resource = My.Resources.frFR.ResourceManager
            End Select
        Else
            resource = My.Resources.enEN.ResourceManager
        End If
    End Sub

    Private Function CreateTexts() As String()
        Dim labelTexts As String() = New String() {resource.GetString("carrier"), resource.GetString("battleship"), resource.GetString("destroyer"), resource.GetString("submarine")}
        Return labelTexts
    End Function

    Public Sub New(menu As MenuScene, sizeX As Integer, sizeY As Integer)
        _updates = New LinkedList(Of IUpdate)
        navalGame = New NavalGame(sizeX, sizeY, PlayerID.Player1)
        Dim n As Integer = GUIController.CurrentContext.NextNegative()
        Dim chanceMapViewerPosition As Vector2 = New Vector2(-Camera.InternalDimensions.X / 2, Camera.InternalDimensions.Y / 2 - 32)
        chanceMapViewer = New ChanceMapViewer(n, 0, n, chanceMapViewerPosition, sizeX, sizeY)
        chanceMapViewer.Scale = Vector2.One * (32.0F / sizeY)

        chanceMapViewer.LayerDetph = 100
        player2IA = New AI1Player(chanceMapViewer, battleShipNum, carrierNum, destroyerNum, submarineNum)
        player2IAMap = New AI1Map()
        Me.sizeX = sizeX
        Me.sizeY = sizeY
        Me.menu = menu
        GUIController.CurrentContext.Add(chanceMapViewer)
        GUIController.CurrentContext.OnFire2 = AddressOf SeeMap
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
            GUIController.UnlockChangeContext()
            GUIController.GoBack()
            GUIController.LockChangeContext()
            ControlsViewPutShipMode()
        Else
            GUIController.UnlockChangeContext()
        End If
    End Sub

    Private Sub Fire0MapButton(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If navalGame.CurrentPlayer <> PlayerID.Player1 Then
            Return
        End If
        If toPut <> Ship.None AndAlso (battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 OrElse carrierNum > 0) Then
            If navalGame.IsPuttable(obj.IndexX, obj.IndexY, toPut, orientation) Then
                navalGame.PutShip(obj.IndexX, obj.IndexY, toPut, orientation)
                Dim num As Integer
                Select Case toPut
                    Case Ship.Carrier
                        carrierNum -= 1
                        num = carrierNum
                    Case Ship.Battleship
                        battleShipNum -= 1
                        num = battleShipNum
                    Case Ship.Destroyer
                        destroyerNum -= 1
                        num = destroyerNum
                    Case Ship.Submarine
                        submarineNum -= 1
                        num = submarineNum
                End Select
                Dim current As Button = putShipContext.GetCurrent()
                If num = 0 Then
                    current.GUIObjectEnable = False
                End If
                current.Label.Text = num.ToString() + current.Label.Text.Substring(1)
            End If
            If carrierNum > 0 OrElse battleShipNum > 0 OrElse destroyerNum > 0 OrElse submarineNum > 0 Then
                GUIController.UnlockChangeContext()
                GUIController.ChangeContext(putShipContext)
                GUIController.LockChangeContext()
                ControlsViewPutShipMenuMode()
            Else
                GUIController.UnlockChangeContext()
                toPut = Ship.None
                ControlsViewGameMode()
                navalGame.Start()
            End If
            navalGame.FillMap(navalMap)
        ElseIf Not endGame Then
            navalGame.Attack(obj.IndexX, obj.IndexY)
            shoot.Play(menu.Volume, 0, 0)
            navalGame.FillMap(navalMap)
            If navalGame.CurrentPlayer <> PlayerID.Player1 Then
                GUIController.CurrentContext.MovableCursor = False
            End If
        End If
        UpdateLabelName()
    End Sub

    Private Sub Fire1MapButton(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
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

    Private Sub Fire2MapButton(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)

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
        putShipContext = New GUIContext(Camera.InternalDimensions)
        For i As Integer = 0 To 3
            Dim num As Integer
            Select Case i
                Case 0
                    num = carrierNum
                Case 1
                    num = battleShipNum
                Case 2
                    num = destroyerNum
                Case 3
                    num = submarineNum
            End Select

            Dim button As Button = New Button(i, 0, i, New Vector2(0, (i - 1) * 32), num.ToString + " " + labelTexts(i), Vector2.One)
            button.LayerDetph = 10
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
            finalText.Label.Text = resource.GetString("you_won")
            finalText.Label.Color = Color.DarkGreen
        Else
            'Lose
            finalText.Label.Text = resource.GetString("you_lost")
            finalText.Label.Color = Color.DarkRed
        End If
        Return finalText
    End Function

    Private Sub CreateAIStatusView()
        Dim position As Vector2 = Vector2.Zero
        position.X = Camera.InternalDimensions.X / 2.0F - 20.0F
        position.Y = Camera.InternalDimensions.Y / 2.0F - 16.0F

        Dim n As Integer = GUIController.CurrentContext.NextNegative()
        Dim statusView As AIStatusViewer = New AIStatusViewer(n, n, n, position, naval, player2IA)
        statusView.Scale = 2 * Vector2.One
        GUIController.CurrentContext.Add(statusView)
    End Sub

    Private Sub CreateLabel()
        Dim n As Integer = GUIController.CurrentContext.NextNegative()
        Dim position As Vector2 = Vector2.Zero
        position.Y = Camera.InternalDimensions.Y / 2.0F - 8.0F
        playerLabel = New GUILabel(n, 0, n, position, New Label("Label", Color.White, Label.Font))
        GUIController.CurrentContext.Add(playerLabel)
    End Sub

    Private Sub UpdateLabelName()
        Select Case navalGame.CurrentPlayer
            Case PlayerID.Player1
                playerLabel.Label.Text = resource.GetString("player_1")
            Case PlayerID.Player2
                playerLabel.Label.Text = resource.GetString("player_2")
        End Select
    End Sub

    Public Overrides Sub LoadContent()
        SetLanguage()
        MyBase.LoadContent()
        labelTexts = CreateTexts()
        CreateControlsViewer()
        CreatePutShipContext()
        CreateLabel()
        naval = content.Load(Of Texture2D)("naval")
        naval2 = content.Load(Of Texture2D)("naval2")
        shoot = content.Load(Of SoundEffect)("tiro")
        CreateAIStatusView()

        Dim area As Vector2 = New Vector2(Camera.InternalDimensions.X, Camera.InternalDimensions.Y - 32)
        navalMap = New NavalMap(GUIController.CurrentContext, naval, naval2, area, sizeX, sizeY, AddressOf Fire0MapButton, AddressOf Fire1MapButton, AddressOf Fire2MapButton, AddressOf CalculateColor)
        navalMap.Position = New Vector2(0, -16.0F)

        navalGame.FillMap(navalMap)

        GUIController.ChangeContext(putShipContext)
        GUIController.LockChangeContext()
        ControlsViewPutShipMenuMode()

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
            If Not selectedShot Then
                If Not player2IA.IsInProcessing And Not player2IA.IsProcessingComplete() Then
                    player2IA.StartAttackProcessing(navalGame.GetEnemyVisionMap(PlayerID.Player1), sizeX, sizeY)
                ElseIf navalGame.CurrentPlayer = PlayerID.Player2 And player2IA.IsProcessingComplete() Then
                    Dim shoot As (x As Integer, y As Integer) = player2IA.NextResult()
                    If navalGame.GetEnemyVisionMap(PlayerID.Player1)(shoot.x + shoot.y * sizeX) = HouseStatus.Normal Then
                        GUIController.CurrentContext.SelectObject(shoot.x, shoot.y)
                        selectedShot = True
                        navalGame.FillMap(navalMap)
                    End If
                End If
            Else
                count += gameTime.ElapsedGameTime.TotalSeconds
                If count > 1.0 Then
                    Dim current As GUIObject = GUIController.CurrentContext.GetCurrent()
                    shoot.Play(menu.Volume, 0, 0)
                    navalGame.Attack(current.IndexX, current.IndexY)
                    navalGame.FillMap(navalMap)
                    count = 0.0
                    selectedShot = False
                    If navalGame.CurrentPlayer = PlayerID.Player1 Then
                        GUIController.CurrentContext.MovableCursor = True
                    End If
                End If
            End If

            UpdateLabelName()
            If navalGame.IsEnd() AndAlso navalGame.GetWin() <> PlayerID.Undefined Then
                winAndLoseContext = New GUIContext(Camera.InternalDimensions)
                GUIController.ChangeContext(winAndLoseContext)

                Dim winAndLoseLabel As GUILabel = CreateWinAndLoseLabel()
                Dim painelSize As Vector2 = winAndLoseLabel.Measure() + 2 * Vector2.One
                Dim colorPainel As ColorPainel = New ColorPainel(GUIController.CurrentContext.NextNegative(), 0, 0, -painelSize / 2.0F, painelSize, Color.GhostWhite)

                Dim labelBPosition As Vector2 = winAndLoseLabel.Position + New Vector2(0, winAndLoseLabel.Measure().Y / 2 + 32)
                Dim guiLabelB As GUILabel = New GUILabel(GUIController.CurrentContext.NextNegative(), 0, 0, labelBPosition, New Label(resource.GetString("return_menu"), Color.YellowGreen, Label.Font))
                Dim painelSizeB As Vector2 = guiLabelB.Measure() + 2 * Vector2.One
                Dim colorPainelB As ColorPainel = New ColorPainel(GUIController.CurrentContext.NextNegative(), 0, 0, guiLabelB.Position - painelSizeB / 2.0F, painelSizeB, Color.GhostWhite)

                Dim fireBPosition As Vector2 = labelBPosition + Vector2.UnitX * (guiLabelB.MeasureIndex(0, 6).X - guiLabelB.Measure().X / 2)
                Dim fireB As GUISprite = New GUISprite(GUIController.CurrentContext.NextNegative(), 0, 0, fireBPosition, GUIController.CreateFireSprite(Color.YellowGreen))
                Dim fireBScale As Single = guiLabelB.MeasureIndex(5, 1).X / fireB.Sprite.Frame.source.Width
                fireB.Scale = Vector2.One * fireBScale

                Dim labelMPosition As Vector2 = guiLabelB.Position + New Vector2(0, guiLabelB.Measure().Y / 2 + 16)
                Dim guiLabelM As GUILabel = New GUILabel(GUIController.CurrentContext.NextNegative(), 0, 0, labelMPosition, New Label(resource.GetString("see_maps"), Color.DarkGoldenrod, Label.Font))
                Dim painelSizeM As Vector2 = guiLabelM.Measure() + 2 * Vector2.One
                Dim colorPainelM As ColorPainel = New ColorPainel(GUIController.CurrentContext.NextNegative(), 0, 0, guiLabelM.Position - painelSizeM / 2.0F, painelSizeM, Color.GhostWhite)

                Dim fireMPosition As Vector2 = labelMPosition + Vector2.UnitX * (guiLabelM.MeasureIndex(0, 6).X - guiLabelM.Measure().X / 2)
                Dim fireM As GUISprite = New GUISprite(GUIController.CurrentContext.NextNegative(), 0, 0, fireMPosition, GUIController.CreateFireSprite(Color.DarkGoldenrod))
                Dim fireMScale As Single = guiLabelM.MeasureIndex(5, 1).X / fireM.Sprite.Frame.source.Width
                fireM.Scale = Vector2.One * fireBScale

                fireB.LayerDetph = 17
                fireM.LayerDetph = 17

                winAndLoseLabel.LayerDetph = 16
                guiLabelB.LayerDetph = 16
                guiLabelM.LayerDetph = 16

                colorPainel.LayerDetph = 15
                colorPainelB.LayerDetph = 15
                colorPainelM.LayerDetph = 15

                GUIController.CurrentContext.Add(fireB)
                GUIController.CurrentContext.Add(fireM)

                GUIController.CurrentContext.Add(winAndLoseLabel)
                GUIController.CurrentContext.Add(guiLabelB)
                GUIController.CurrentContext.Add(guiLabelM)

                GUIController.CurrentContext.Add(colorPainel)
                GUIController.CurrentContext.Add(colorPainelB)
                GUIController.CurrentContext.Add(colorPainelM)

                endGame = True
                GUIController.CurrentContext.CursorEnable = False
                navalMap.RemoveButtons()
                GUIController.CurrentContext.OnFire0 = AddressOf Back
                GUIController.CurrentContext.OnFire2 = AddressOf SeeMap
            End If
        End If
    End Sub

    Private Sub Back(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If endGame Then
            ScreenManager.Instance.ChangeScene(Me.menu)
        End If
    End Sub

    Dim seeMapPlayer As PlayerID = PlayerID.Undefined

    Private Sub SeeMap(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If endGame Then
            If seeMapPlayer = PlayerID.Undefined AndAlso GUIController.CurrentContext IsNot GUIController.MainContext Then
                seeMapPlayer = PlayerID.Player1
                GUIController.GoBack()
                ControlsViewSeeMapMode()
            ElseIf seeMapPlayer = PlayerID.Player1 OrElse (GUIController.CurrentContext Is GUIController.MainContext And seeMapPlayer <> PlayerID.Player2) Then
                seeMapPlayer = PlayerID.Player2
            ElseIf seeMapPlayer = PlayerID.Player2 Then
                seeMapPlayer = PlayerID.Undefined
                GUIController.ChangeContext(winAndLoseContext)
            End If
            navalGame.FillMap(navalMap, seeMapPlayer, True)
        End If
    End Sub

    Private Sub ControlsViewPutShipMode()
        GUIController.CurrentContext.Add(controlsView)

        labelMove.DrawEnable = True
        labelMoveMenu.DrawEnable = False

        labelFire0.Text = resource.GetString("place")
        labelFire0.DrawEnable = True

        labelFire1.Text = resource.GetString("rotate")
        labelFire1.DrawEnable = True

        labelFire2.DrawEnable = False
    End Sub

    Private Sub ControlsViewSeeMapMode()
        labelMove.DrawEnable = False

        labelMoveMenu.DrawEnable = False

        labelFire0.DrawEnable = False

        labelFire1.DrawEnable = False

        labelFire2.DrawEnable = False
    End Sub

    Private Sub ControlsViewPutShipMenuMode()
        GUIController.CurrentContext.Add(controlsView)

        labelMove.DrawEnable = False
        labelMoveMenu.DrawEnable = True

        labelFire0.Text = resource.GetString("select")
        labelFire0.DrawEnable = True

        labelFire1.DrawEnable = False

        labelFire2.DrawEnable = False
    End Sub

    Private Sub ControlsViewGameMode()
        GUIController.CurrentContext.Add(controlsView)

        labelMove.DrawEnable = True

        labelMoveMenu.DrawEnable = False

        labelFire0.Text = resource.GetString("attack")

        labelFire0.DrawEnable = True

        labelFire1.DrawEnable = False

        labelFire2.DrawEnable = True
    End Sub

    Private Sub CreateControlsViewer()
        Dim n As Integer = GUIController.MainContext.NextNegative()
        controlsView = New ControlsViewer(n, 0, n, Camera.InternalDimensions / 2 - Vector2.UnitX * 64)
        controlsView.Scale = Vector2.One * 0.5F

        ''Move
        labelMove = New Label(resource.GetString("move"), Color.White, Label.Font)

        Dim analogicMove As Sprite = GUIController.CreateAnalogicSprite(Color.BlanchedAlmond)
        Dim up As Sprite = GUIController.CreateUpSprite(Color.BlanchedAlmond)
        Dim down As Sprite = GUIController.CreateDownSprite(Color.BlanchedAlmond)
        Dim left As Sprite = GUIController.CreateLeftSprite(Color.BlanchedAlmond)
        Dim right As Sprite = GUIController.CreateRightSprite(Color.BlanchedAlmond)

        Dim moveList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        moveList.Add((analogicMove, Nothing))
        moveList.Add((down, Nothing))
        moveList.Add((up, Nothing))
        moveList.Add((right, Nothing))
        moveList.Add((left, Nothing))

        controlsView.Add(labelMove, moveList)

        ''Move menu
        labelMoveMenu = New Label(resource.GetString("move"), Color.White, Label.Font)

        Dim moveMenuList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        moveMenuList.Add((analogicMove, Nothing))
        moveMenuList.Add((down, Nothing))
        moveMenuList.Add((up, Nothing))

        controlsView.Add(labelMoveMenu, moveMenuList)
        ''Fire0
        labelFire0 = New Label(resource.GetString("place"), Color.White, Label.Font)

        Dim Fire0 As Sprite = GUIController.CreateFireSprite(Color.GreenYellow)
        Dim orbFire0 As Sprite = GUIController.CreateOrbSprite(Color.GreenYellow)
        Dim orbFire0Label As Label = New Label("B", Color.White, Label.Font)


        Dim fire0List As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        fire0List.Add((Fire0, Nothing))
        fire0List.Add((orbFire0, orbFire0Label))

        controlsView.Add(labelFire0, fire0List)

        ''Fire1
        labelFire1 = New Label(resource.GetString("rotate"), Color.White, Label.Font)

        Dim Fire1 As Sprite = GUIController.CreateFireSprite(Color.DarkRed)
        Dim orbFire1 As Sprite = GUIController.CreateOrbSprite(Color.DarkRed)
        Dim orbFire1Label As Label = New Label("N", Color.White, Label.Font)


        Dim fire1List As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        fire1List.Add((Fire1, Nothing))
        fire1List.Add((orbFire1, orbFire1Label))

        controlsView.Add(labelFire1, fire1List)

        ''Fire2
        labelFire2 = New Label(resource.GetString("switch_map"), Color.White, Label.Font)

        Dim Fire2 As Sprite = GUIController.CreateFireSprite(Color.DarkOrange)
        Dim orbFire2 As Sprite = GUIController.CreateOrbSprite(Color.DarkOrange)
        Dim orbFire2Label As Label = New Label("M", Color.White, Label.Font)

        Dim fire2List As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        fire2List.Add((Fire2, Nothing))
        fire2List.Add((orbFire2, orbFire2Label))

        controlsView.Add(labelFire2, fire2List)

        GUIController.CurrentContext.Add(controlsView)
    End Sub

End Class
