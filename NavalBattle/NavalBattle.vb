Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class NavalBattle
    Inherits Microsoft.Xna.Framework.Game

    Private graphics As GraphicsDeviceManager
    Private spriteBatch As SpriteBatch
    Private resolution As Vector2

    Private camera As Camera
    Dim sprite As AnimatedSprite(Of Integer)

    Private updateList As LinkedList(Of IUpdate)

    ''' <summary>
    ''' Cria novo jogo
    ''' </summary>
    ''' <param name="resolution"> Resolução da tela </param>
    Public Sub New(ByVal resolution As Vector2)
        Me.graphics = New GraphicsDeviceManager(Me)
        Me.resolution = resolution
        Content.RootDirectory = "Content"
        updateList = New LinkedList(Of IUpdate)
    End Sub

    ''' <summary>
    ''' Allows the game to perform any initialization it needs to before starting to run.
    ''' This Is where it can query for any required services And load any non-graphic
	''' related content.  Calling base.Initialize will enumerate through any components
	''' And initialize them as well.
    ''' </summary>
    Protected Overrides Sub Initialize()
        graphics.PreferredBackBufferWidth = resolution.X
        graphics.PreferredBackBufferHeight = resolution.Y
        graphics.ApplyChanges()
        MyBase.Initialize()
    End Sub

    ''' <summary>
    ''' LoadContent will be called once per game And Is the place to load
	''' all of your content.
    ''' </summary>
    Protected Overrides Sub LoadContent()
        spriteBatch = New SpriteBatch(GraphicsDevice)
        Me.camera = New Camera(resolution, resolution / 2.0F, spriteBatch)
        Dim naval As Texture2D = Content.Load(Of Texture2D)("naval")
        sprite = New AnimatedSprite(Of Integer)(New Vector2(0, 0))

        Dim l As List(Of Frame) = New List(Of Frame)
        For i = 0 To 8
            For j = 0 To 8
                l.Add(New Frame(naval, Vector2.Zero, New Rectangle(i * 16, j * 16, 16, 16), Color.White, SpriteEffects.None))
            Next
        Next

        Dim animation As Animation = New Animation(l)
        sprite.Animations.Add(0, animation)
        sprite.animation = 0

        updateList.AddLast(sprite)
        Camera.Drawings.Add(sprite)
    End Sub

    Protected Overrides Sub UnloadContent()
        Content.Unload()
    End Sub

    ''' <summary>
	''' Allows the game to run logic such as updating the world,
	''' checking for collisions, gathering input, And playing audio.
	''' </summary>
	''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Update(gameTime As GameTime)
        If GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed OrElse Keyboard.GetState().IsKeyDown(Keys.Escape) Then
            Me.Exit()
        End If

        MyBase.Update(gameTime)

        For Each update As IUpdate In updateList
            update.Update(gameTime)
        Next
    End Sub

    ''' <summary>
	''' This Is called when the game should draw itself.
	''' </summary>
	''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Draw(gameTime As GameTime)
        GraphicsDevice.Clear(Color.CornflowerBlue)
        Me.camera.Draw()
        MyBase.Draw(gameTime)
    End Sub

End Class
