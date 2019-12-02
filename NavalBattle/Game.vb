Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class Game
    Inherits Microsoft.Xna.Framework.Game

    Public ReadOnly Property Graphics As GraphicsDeviceManager
<<<<<<< HEAD
    Public language As String
=======
>>>>>>> 170a8539d797b451d7c850b769592cb02d9f9711
    Private spriteBatch As SpriteBatch

    ''' <summary>
    ''' Cria novo jogo
    ''' </summary>
    Public Sub New()
        Me.graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
    End Sub

    ''' <summary>
    ''' Allows the game to perform any initialization it needs to before starting to run.
    ''' This Is where it can query for any required services And load any non-graphic
	''' related content.  Calling base.Initialize will enumerate through any components
	''' And initialize them as well.
    ''' </summary>
    Protected Overrides Sub Initialize()

        MyBase.Initialize()
    End Sub

    ''' <summary>
    ''' LoadContent will be called once per game And Is the place to load
	''' all of your content.
    ''' </summary>
    Protected Overrides Sub LoadContent()
        spriteBatch = New SpriteBatch(GraphicsDevice)
        ScreenManager.Instance.SpriteBatch = spriteBatch
        ScreenManager.Instance.Game = Me
        ScreenManager.Instance.Dimensions = New Vector2(1024, 576)
        ScreenManager.Instance.LoadContent(Content)
        ScreenManager.Instance.ChangeScene(New MenuScene(Me))
    End Sub

    Protected Overrides Sub UnloadContent()
        ScreenManager.Instance.UnloadContent()
        Content.Unload()
    End Sub

    ''' <summary>
	''' Allows the game to run logic such as updating the world,
	''' checking for collisions, gathering input, And playing audio.
	''' </summary>
	''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Update(gameTime As GameTime)

        ScreenManager.Instance.Update(gameTime)

        If GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed OrElse Keyboard.GetState().IsKeyDown(Keys.Escape) Then
            Me.Exit()
        End If

        MyBase.Update(gameTime)
    End Sub

    ''' <summary>
	''' This Is called when the game should draw itself.
	''' </summary>
	''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Draw(gameTime As GameTime)
        GraphicsDevice.Clear(ScreenManager.BackgroundColor)
        ScreenManager.Instance.Draw(spriteBatch)
        MyBase.Draw(gameTime)
    End Sub

End Class
