Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class NavalBattle
    Inherits Microsoft.Xna.Framework.Game

    Private graphics As GraphicsDeviceManager
    Private spriteBatch As SpriteBatch

    Private camera As Camera
    Dim sprite As AnimatedSprite(Of Integer)
    Dim box As Box
    Dim guiController As GUIContext

    Private updateList As LinkedList(Of IUpdate)

    ''' <summary>
    ''' Cria novo jogo
    ''' </summary>
    Public Sub New()
        Me.graphics = New GraphicsDeviceManager(Me)
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
        graphics.PreferredBackBufferWidth = ScreenManager.Instance.Dimensions.X
        graphics.PreferredBackBufferHeight = ScreenManager.Instance.Dimensions.Y
        graphics.ApplyChanges()
        MyBase.Initialize()
    End Sub

    ''' <summary>
    ''' LoadContent will be called once per game And Is the place to load
	''' all of your content.
    ''' </summary>
    Protected Overrides Sub LoadContent()
        spriteBatch = New SpriteBatch(GraphicsDevice)

        ScreenManager.Instance.LoadContent(Content)


        Me.camera = New Camera(ScreenManager.Instance.Dimensions, ScreenManager.Instance.Dimensions / 2.0F, spriteBatch)
        Dim naval As Texture2D = Content.Load(Of Texture2D)("naval")
        sprite = New AnimatedSprite(Of Integer)(New Vector2(0, 0))

        Dim l As List(Of Frame) = New List(Of Frame)
        For i = 0 To 8
            For j = 0 To 8
                Dim frame As Frame = New Frame(naval, Vector2.Zero, New Rectangle(i * 16, j * 16, 16, 16), Color.White, SpriteEffects.None)
                frame.infinity = True
                l.Add(frame)
            Next
        Next

        Dim animation As Animation = New Animation(l)
        sprite.Animations.Add(0, animation)
        sprite.animation = 0
        sprite.Scale = New Vector2(4.5, 4.5)

        ''updateList.AddLast(sprite)
        'sprite.SetFrameIndex(0)
        'Camera.Drawings.Add(sprite)


        Dim topLeft = New Sprite()
        topLeft.Frame = New Frame(naval, Vector2.Zero, New Rectangle(64, 96, 8, 8), Color.White, SpriteEffects.None)

        Dim topRight = New Sprite()
        topRight.Frame = New Frame(naval, Vector2.Zero, New Rectangle(80, 96, 8, 8), Color.White, SpriteEffects.None)

        Dim downLeft = New Sprite()
        downLeft.Frame = New Frame(naval, Vector2.Zero, New Rectangle(64, 112, 8, 8), Color.White, SpriteEffects.None)

        Dim downRight = New Sprite()
        downRight.Frame = New Frame(naval, Vector2.Zero, New Rectangle(80, 112, 8, 8), Color.White, SpriteEffects.None)

        Dim top = New Sprite()
        top.Frame = New Frame(naval, Vector2.Zero, New Rectangle(72, 96, 8, 8), Color.White, SpriteEffects.None, True)
        Dim down = New Sprite()
        down.Frame = New Frame(naval, Vector2.Zero, New Rectangle(72, 112, 8, 8), Color.White, SpriteEffects.None, True)
        Dim left = New Sprite()
        left.Frame = New Frame(naval, Vector2.Zero, New Rectangle(64, 104, 8, 8), Color.White, SpriteEffects.None, True)
        Dim right = New Sprite()
        right.Frame = New Frame(naval, Vector2.Zero, New Rectangle(80, 104, 8, 8), Color.White, SpriteEffects.None, True)
        Dim middler = New Sprite()
        middler.Frame = New Frame(naval, Vector2.Zero, New Rectangle(72, 104, 8, 8), Color.White, SpriteEffects.None, True)


        box = New Box(New Vector2(24, 24), topLeft, topRight, downLeft, downRight, left, right, top, down, middler)
        Camera.Drawings.Add(box)

        Dim tile As Tilemap(Of Integer) = New Tilemap(Of Integer)(10, 10)
        tile.LayerDepth = 0.4
        For i = 0 To 8
            For j = 0 To 8
                Dim sprite As Sprite = New Sprite()
                sprite.Frame = New Frame(naval, Vector2.Zero, New Rectangle(i * 16, j * 16, 16, 16), Color.White, SpriteEffects.None)
                tile.Sprites.Add(i + 9 * j, sprite)
                If i + j = 0 Then
                    'sprite.DrawEnable = False
                End If
            Next
        Next
        tile.SetIndex(1, 1, 2)

        'Camera.Drawings.Add(tile)
        'Dim screenRectangle As Rectangle = New Rectangle(camera.Position.X - camera.InternalDimensions.X / 2, camera.Position.Y - camera.InternalDimensions.Y / 2, camera.InternalDimensions.X, camera.InternalDimensions.Y)
        Dim mapSize As Vector2 = camera.InternalDimensions
        guiController = New GUIContext(mapSize, New Frame(naval, New Vector2(4, 4), New Rectangle(80, 120, 8, 8), Color.White, SpriteEffects.None),
                                          New Frame(naval, New Vector2(4, 4), New Rectangle(88, 120, 8, 8), Color.White, SpriteEffects.None))
        For i As Integer = 0 To 10
            For j As Integer = 0 To 10
                'guiController.Add(New SimpleButton(i + j * 11, i, j, New Vector2(16 * i - 88, 16 * j - 88), New Frame(naval, Vector2.Zero, New Rectangle(144, 144, 8, 8), Color.White, SpriteEffects.None)))
            Next
        Next
        Camera.Drawings.Add(guiController)
        updateList.AddLast(guiController)

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

        For Each update As IUpdate In updateList
            update.Update(gameTime)
        Next

        If box.Size.Length <= 248 Then
            box.Size = box.Size + Vector2.One * gameTime.ElapsedGameTime.TotalSeconds * 224
        End If
        'box.Angle += gameTime.ElapsedGameTime.TotalSeconds * 20



        If Keyboard.GetState().IsKeyDown(Keys.C) Then
            box.Size = New Vector2(24, 24)
        End If

        If Keyboard.GetState().IsKeyDown(Keys.X) Then
            box.Scale = box.Scale + Vector2.One * gameTime.ElapsedGameTime.TotalSeconds
        ElseIf Keyboard.GetState().IsKeyDown(Keys.Z) Then
            box.Scale = box.Scale - Vector2.One * gameTime.ElapsedGameTime.TotalSeconds
        End If

    End Sub

    ''' <summary>
	''' This Is called when the game should draw itself.
	''' </summary>
	''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Draw(gameTime As GameTime)
        GraphicsDevice.Clear(Color.Black)
        ScreenManager.Instance.Draw(spriteBatch)
        Me.camera.Draw()
        MyBase.Draw(gameTime)
    End Sub

End Class
