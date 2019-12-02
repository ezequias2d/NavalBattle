Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Audio
Imports Microsoft.Xna.Framework.Graphics
Public Class MenuScene
    Inherits GameScene

    Private naval As Texture2D

    Private texts As String()
    Public game As Game

    Private contextSizeMap As GUIContext
    Private contextSettings As GUIContext

    Private numberSelectorX As NumberSelector
    Private numberSelectorY As NumberSelector

    Private check As Check
    Private resolutionAlternator As SimpleAlternator(Of (x As UInteger, y As UInteger))
    Private numberSelectorVolume As NumberSelector
    Private languageAlternator As SimpleAlternator(Of String)

    Private controlsView As ControlsViewer
    Private labelMove As Label
    Private labelFire0 As Label
    Private labelCancel As Label
    Private labelChangeValue As Label

    Private sound As SoundEffect
    Private soundEffectInstance As SoundEffectInstance

    Public resource As Resources.ResourceManager

    Dim bk As BackgroundMenu

    Public ReadOnly Property Volume As Single
        Get
            Return numberSelectorVolume.Value * 0.05F
        End Get
    End Property

    Public Sub New(ByRef game As Game)
        _updates = New LinkedList(Of IUpdate)
        Me.game = game
    End Sub

    Private Sub SetLanguage()
        If game IsNot Nothing AndAlso game.language IsNot Nothing Then
            Select Case (game.language)
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

    Private Function GetString(str As String) As String
        Return resource.GetString(str)
    End Function

    Private Function CreateTexts() As String()
        Dim texts As String() = New String() {"play", "settings", "exit"}
        Return texts
    End Function

    Private Sub StartButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        GUIController.ChangeContext(contextSizeMap)
        ControlsViewSizeMap()
    End Sub

    Private Sub SettingsButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        GUIController.ChangeContext(contextSettings)
    End Sub

    Private Sub OkButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        Dim navalBattle As NavalBattleScene = New NavalBattleScene(Me, numberSelectorX.Value, numberSelectorY.Value)
        navalBattle.Language = Me.Language
        ScreenManager.Instance.ChangeScene(navalBattle)
        ScreenManager.Instance.Current.Updates.Add(bk)
        ScreenManager.Instance.Current.Updates.Add(bk)
        Camera.Drawings.Add(bk)
        bk.EnableLogo = False
    End Sub

    Private Sub OnFocusNumberSelector(obj As GUIObject)
        ControlsViewSizeMapSelect()
    End Sub

    Private Sub OnFocusMainContext(obj As GUIObject)
        ControlsViewMainMenuMode()
    End Sub

    Private Sub OnUnfocusNumberSelector(obj As GUIObject)
        ControlsViewSizeMap()
    End Sub

    Private Sub ExitButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        game.Exit()
    End Sub

    Private Function CreateContextSettings() As GUIContext
        Dim context As GUIContext = New GUIContext(Camera.InternalDimensions)

        Dim n As Integer = context.NextNegative()

        Dim resolutionLabel As GUILabel = New GUILabel(n, 0, n, New Vector2(0, -110), New Label("resolution", Color.White, Label.Font))
        resolutionLabel.GetStringFunction = AddressOf GetString

        Dim resolutions As IDictionary(Of String, (x As UInteger, y As UInteger)) = New Dictionary(Of String, (x As UInteger, y As UInteger))

        resolutions.Add("640x360", (640, 360))
        resolutions.Add("1024x576", (1024, 576))
        resolutions.Add("1280x720", (1280, 720))
        resolutions.Add("1366x768", (1366, 768))
        resolutions.Add("1600x900", (1600, 900))
        resolutions.Add("1920x1080", (1920, 1080))
        resolutions.Add("2560x1600", (2560, 1600))
        resolutions.Add("3840x2160", (3840, 2160))

        resolutionAlternator = New SimpleAlternator(Of (x As UInteger, y As UInteger))(0, 0, 0, New Vector2(0, -90), resolutions)

        n = context.NextNegative()
        Dim fullscreenLabel As GUILabel = New GUILabel(n, 0, n, New Vector2(0, -60), New Label("fullscreen", Color.White, Label.Font))
        fullscreenLabel.GetStringFunction = AddressOf GetString
        check = New Check(1, 0, 1, New Vector2(0, -30))

        n = context.NextNegative()
        Dim volumeLabel As GUILabel = New GUILabel(n, 0, n, Vector2.Zero, New Label("volume", Color.White, Label.Font))
        volumeLabel.GetStringFunction = AddressOf GetString

        numberSelectorVolume = New NumberSelector(2, 0, 2, New Vector2(0, 30), 0, 20)

        numberSelectorVolume.Value = 10

        n = context.NextNegative()
        Dim languageLabel As GUILabel = New GUILabel(n, 0, n, New Vector2(0, 60), New Label("language", Color.White, Label.Font))
        languageLabel.GetStringFunction = AddressOf GetString
        Dim languages As IDictionary(Of String, String) = New Dictionary(Of String, String)

        languages.Add("English", "en-EN")
        languages.Add("Portugues", "pt-BR")
        languages.Add("Francais", "fr-FR")
        languages.Add("Espanol", "es-ES")

        languageAlternator = New SimpleAlternator(Of String)(3, 0, 3, New Vector2(0, 90), languages)

        context.Add(resolutionLabel)
        context.Add(resolutionAlternator)
        context.Add(fullscreenLabel)
        context.Add(check)
        context.Add(volumeLabel)
        context.Add(numberSelectorVolume)
        context.Add(languageLabel)
        context.Add(languageAlternator)

        resolutionLabel.LayerDetph = 10
        resolutionAlternator.LayerDetph = 10
        fullscreenLabel.LayerDetph = 10
        check.LayerDetph = 10
        volumeLabel.LayerDetph = 10
        numberSelectorVolume.LayerDetph = 10
        languageLabel.LayerDetph = 10
        languageAlternator.LayerDetph = 10

        resolutionAlternator.OnUnfocus = AddressOf OnUnfocusResolutionAlternator
        numberSelectorVolume.OnUnfocus = AddressOf OnUnfocusNumberSelectorVolume
        check.OnFire0 = AddressOf OnFire0FullScreen
        languageAlternator.OnUnfocus = AddressOf OnUnfocusLanguageAlternator
        Return context
    End Function

    Private Sub OnFire0FullScreen(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        Dim check As Check = obj
        check.Value = Not check.Value
        ScreenManager.Instance.FullScreen = check.Value
    End Sub

    Private Sub OnUnfocusResolutionAlternator(obj As GUIObject)
        Dim resolutionAlternator As SimpleAlternator(Of (x As UInteger, y As UInteger)) = obj
        Dim resolution As (x As UInteger, y As UInteger) = resolutionAlternator.Value
        ScreenManager.Instance.Dimensions = New Vector2(resolution.x, resolution.y)
    End Sub

    Private Sub OnUnfocusNumberSelectorVolume(obj As GUIObject)
        soundEffectInstance.Volume = numberSelectorVolume.Value * 0.05F
    End Sub

    Private Sub OnUnfocusLanguageAlternator(obj As GUIObject)
        Dim languageAlternator As SimpleAlternator(Of String) = obj
        language = languageAlternator.Value
        game.language = Language
        SetLanguage()
    End Sub

    Private Function CreateContextSizeMap() As GUIContext
        Dim context As GUIContext = New GUIContext(Camera.InternalDimensions)

        numberSelectorX = New NumberSelector(0, 0, 0, Vector2.Zero, 6, 20)
        numberSelectorY = New NumberSelector(1, 0, 1, Vector2.Zero, 6, 20)
        Dim button As Button = New Button(2, 0, 2, Vector2.Zero, "Ok!", Vector2.One)

        Dim gLabelTitle As GUILabel = New GUILabel(3, 0, 3, New Vector2(0, -48.0F), New Label("map_size", Color.White, Label.Font))

        Dim gLabelW As GUILabel = New GUILabel(4, 0, 4, New Vector2(0, -20.0F), New Label("width", Color.White, Label.Font))
        Dim gLabelH As GUILabel = New GUILabel(5, 0, 5, New Vector2(0, 28.0F), New Label("height", Color.White, Label.Font))

        gLabelTitle.GetStringFunction = AddressOf GetString
        gLabelW.GetStringFunction = AddressOf GetString
        gLabelH.GetStringFunction = AddressOf GetString

        gLabelTitle.Scale = Vector2.One * 2.0F
        gLabelW.Scale = Vector2.One * 0.5F
        gLabelH.Scale = Vector2.One * 0.5F

        numberSelectorX.Position = New Vector2(0, 0)
        numberSelectorY.Position = New Vector2(0, 48)

        numberSelectorX.Value = 10
        numberSelectorY.Value = 10
        button.Position = New Vector2(0, numberSelectorY.Position.Y + numberSelectorY.GetSize().Y * 3.0F / 2.0F)

        button.OnFire0 = AddressOf OkButton

        numberSelectorX.OnFocus = AddressOf OnFocusNumberSelector
        numberSelectorY.OnFocus = AddressOf OnFocusNumberSelector

        numberSelectorX.OnUnfocus = AddressOf OnUnfocusNumberSelector
        numberSelectorY.OnUnfocus = AddressOf OnUnfocusNumberSelector

        context.Add(numberSelectorX)
        context.Add(numberSelectorY)
        context.Add(button)
        context.Add(gLabelH)
        context.Add(gLabelW)
        context.Add(gLabelTitle)

        Return context
    End Function

    Private Sub CreateMenu()
        Dim sizePainel As Vector2 = New Vector2(224, 62)

        Dim color As Color = Color.RoyalBlue
        Dim n As Integer = GUIController.MainContext.NextNegative()

        GUIController.MainContext.OnFocus = AddressOf OnFocusMainContext

        Dim labelTitle As GUILabel = New GUILabel(n, 0, n, Vector2.Zero, New Label("naval_battle", Color.GhostWhite, Label.Font))

        labelTitle.GetStringFunction = AddressOf GetString

        labelTitle.LayerDetph = 5
        labelTitle.Scale = Vector2.One * 1.5F
        GUIController.MainContext.Add(labelTitle)

        Dim i As Integer = 0
        For Each text As String In texts
            Dim button As Button = New Button(i, 0, i, New Vector2(0, (i + 4 - texts.Count) * 32.0F), text, Vector2.One)
            button.GetStringFunction = AddressOf GetString
            GUIController.MainContext.Add(button)
            Select Case i
                Case 0
                    button.OnFire0 = AddressOf StartButton
                Case 1
                    button.OnFire0 = AddressOf SettingsButton
                Case 2
                    button.OnFire0 = AddressOf ExitButton
            End Select
            button.LayerDetph = 5
            i += 1
        Next
    End Sub

    Private Sub ControlsViewMainMenuMode()
        GUIController.CurrentContext.Add(controlsView)
        labelMove.DrawEnable = True

        labelFire0.DrawEnable = True
        labelFire0.Text = "execute"

        labelCancel.DrawEnable = False
        labelChangeValue.DrawEnable = False
    End Sub

    Private Sub ControlsViewSizeMap()
        GUIController.CurrentContext.Add(controlsView)
        labelMove.DrawEnable = True

        labelFire0.DrawEnable = True
        labelFire0.Text = "select_execute"

        labelCancel.DrawEnable = True
        labelCancel.Text = "return"

        labelChangeValue.DrawEnable = False
    End Sub

    Private Sub ControlsViewSizeMapSelect()
        GUIController.CurrentContext.Add(controlsView)
        labelMove.DrawEnable = False
        labelFire0.DrawEnable = False

        labelCancel.DrawEnable = True
        labelCancel.Text = "deselect"

        labelChangeValue.DrawEnable = True
    End Sub

    Private Sub CreateControlsViewer()
        Dim n As Integer = GUIController.MainContext.NextNegative()
        controlsView = New ControlsViewer(n, 0, n, Vector2.Zero)
        controlsView.LayerDetph = 5
        controlsView.Scale = Vector2.One * 0.5F
        controlsView.Position = Camera.InternalDimensions / 2 - Vector2.UnitX * 64

        ''Move
        labelMove = New Label("move", Color.White, Label.Font)
        labelMove.GetStringFunction = AddressOf GetString

        Dim analogicMove As Sprite = GUIController.CreateAnalogicSprite(Color.BlanchedAlmond)
        Dim up As Sprite = GUIController.CreateUpSprite(Color.BlanchedAlmond)
        Dim down As Sprite = GUIController.CreateDownSprite(Color.BlanchedAlmond)

        Dim moveList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        moveList.Add((analogicMove, Nothing))
        moveList.Add((down, Nothing))
        moveList.Add((up, Nothing))

        controlsView.Add(labelMove, moveList)

        ''Fire0
        labelFire0 = New Label("execute", Color.White, Label.Font)
        labelFire0.GetStringFunction = AddressOf GetString

        Dim Fire0 As Sprite = GUIController.CreateFireSprite(Color.GreenYellow)
        Dim orbFire0 As Sprite = GUIController.CreateOrbSprite(Color.GreenYellow)
        Dim orbFire0Label As Label = New Label("b", Color.White, Label.Font)


        Dim fire0List As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        fire0List.Add((Fire0, Nothing))
        fire0List.Add((orbFire0, orbFire0Label))

        controlsView.Add(labelFire0, fire0List)

        ''Cancel
        labelCancel = New Label("cancel", Color.White, Label.Font)
        labelCancel.GetStringFunction = AddressOf GetString
        labelCancel.DrawEnable = False

        Dim cancel As Sprite = GUIController.CreateFireSprite(Color.Red)
        Dim orbCancel As Sprite = GUIController.CreateOrbSprite(Color.Red)
        Dim orbCancelLabel As Label = New Label("<-", Color.White, Label.Font)


        Dim cancelList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        cancelList.Add((cancel, Nothing))
        cancelList.Add((orbCancel, orbCancelLabel))

        controlsView.Add(labelCancel, cancelList)

        ''ChangeValue
        labelChangeValue = New Label("change", Color.White, Label.Font)
        labelChangeValue.GetStringFunction = AddressOf GetString
        labelChangeValue.DrawEnable = False

        Dim left As Sprite = GUIController.CreateLeftSprite(Color.DarkBlue)
        Dim right As Sprite = GUIController.CreateRightSprite(Color.DarkRed)

        Dim changeList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        changeList.Add((right, Nothing))
        changeList.Add((left, Nothing))

        controlsView.Add(labelChangeValue, changeList)

        GUIController.MainContext.Add(controlsView)
    End Sub

    Public Overrides Sub Update(gameTime As GameTime)
        MyBase.Update(gameTime)
        bk.EnableLogo = (GUIController.CurrentContext.GetHashCode() = GUIController.MainContext.GetHashCode())
    End Sub

    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        SetLanguage()

        naval = content.Load(Of Texture2D)("naval")

        texts = CreateTexts()

        CreateMenu()

        CreateControlsViewer()

        contextSizeMap = CreateContextSizeMap()

        contextSettings = CreateContextSettings()

        Dim n As Integer = GUIController.CurrentContext.NextNegative()
        bk = New BackgroundMenu(n, 0, n, Vector2.Zero, ScreenManager.Instance.Content.Load(Of Texture2D)("naval2"), Camera.InternalDimensions)

        bk.EnableLogo = True
        Updates.Add(bk)
        Camera.Drawings.Add(bk)

        If sound Is Nothing Then
            sound = ScreenManager.Instance.Content.Load(Of SoundEffect)("somdefundo")
            soundEffectInstance = sound.CreateInstance()
            soundEffectInstance.IsLooped = True
            soundEffectInstance.Play()
            soundEffectInstance.Volume = 0.5F
        End If
        numberSelectorVolume.Value = soundEffectInstance.Volume * 20.0F

        resolutionAlternator.Value = (ScreenManager.Instance.Dimensions.X, ScreenManager.Instance.Dimensions.Y)
        check.Value = ScreenManager.Instance.FullScreen
    End Sub

End Class
