﻿Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Content
Imports Microsoft.Xna.Framework.Graphics

''' <summary>
''' Gerenciador de tela
''' </summary>
Public Class ScreenManager
    Private Shared _Instance As ScreenManager
    Private Shared _Dimensions As Vector2
    Private Shared _FullScreen As Boolean
    Private _Content As ContentManager
    Private _Current As GameScene

    ''' <summary>
    ''' SpriteBatch principal 
    ''' </summary>
    ''' <returns> SpriteBatch </returns>
    Public Property SpriteBatch As SpriteBatch

    ''' <summary>
    ''' Objeto Game
    ''' </summary>
    ''' <returns> Game </returns>
    Public Property Game As Game

    ''' <summary>
    ''' Cena atual
    ''' </summary>
    ''' <returns> GameScene </returns>
    Public Property Current As GameScene
        Get
            Return _Current
        End Get
        Private Set
            _Current = Value
        End Set
    End Property

    ''' <summary>
    ''' ContentManager principal
    ''' </summary>
    ''' <returns> ContentManager </returns>
    Public Property Content As ContentManager
        Get
            Return _Content
        End Get
        Set
            _Content = Value
        End Set
    End Property

    ''' <summary>
    ''' Dimensões da tela
    ''' </summary>
    ''' <returns></returns>
    Public Property Dimensions As Vector2
        Get
            Return _Dimensions
        End Get
        Set
            If Value.X <> 0 AndAlso Value.Y <> 0 Then
                _Dimensions = Value
                Game.Graphics.PreferredBackBufferWidth = Value.X
                Game.Graphics.PreferredBackBufferHeight = Value.Y
                Game.Graphics.ApplyChanges()
            End If
        End Set
    End Property

    Public Property FullScreen As Boolean
        Get
            Return _FullScreen
        End Get
        Set(value As Boolean)
            If Game IsNot Nothing Then
                _FullScreen = value
                Game.Graphics.IsFullScreen = value
                Game.Graphics.ApplyChanges()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Instancia da classe ScreenManager
    ''' </summary>
    ''' <returns> ScreenManager </returns>
    Public Shared ReadOnly Property Instance As ScreenManager
        Get
            If _Instance Is Nothing Then
                _Instance = New ScreenManager()
            End If
            Return _Instance
        End Get
    End Property

    ''' <summary>
    ''' Cor de fundo da tela
    ''' </summary>
    ''' <returns> Color </returns>
    Public Shared Property BackgroundColor As Color

    ''' <summary>
    ''' Cria novo ScreenManager
    ''' </summary>
    Private Sub New()

    End Sub

    ''' <summary>
    ''' Carrega recurso
    ''' </summary>
    ''' <param name="content"></param>
    Public Sub LoadContent(ByRef content As ContentManager)
        Me.Content = New ContentManager(content.ServiceProvider, "Content")
    End Sub

    ''' <summary>
    ''' Muda cena atual
    ''' </summary>
    ''' <param name="newScene"></param>
    Public Sub ChangeScene(ByRef newScene As GameScene)
        If newScene Is Nothing OrElse newScene Is Current Then
            Return
        End If

        If Not Current Is Nothing Then
            newScene.Language = Current.Language
            Current.UnloadContent()
        End If

        newScene.LoadContent()
        Current = newScene
    End Sub

    ''' <summary>
    ''' Descarrega recurso
    ''' </summary>
    Public Sub UnloadContent()
        If Not Current Is Nothing Then
            Current.UnloadContent()
        End If
    End Sub

    ''' <summary>
    ''' Update
    ''' </summary>
    ''' <param name="gameTime"> Tempo de jogo </param>
    Public Sub Update(ByVal gameTime As GameTime)
        If Not Current Is Nothing Then
            Current.Update(gameTime)
        End If
    End Sub

    ''' <summary>
    ''' Draw
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch)
        If Not Current Is Nothing Then
            Current.Draw(spriteBatch)
        End If
    End Sub

    Public Function CreateTexture(width As Integer, height As Integer) As Texture2D
        Dim texture As Texture2D = New Texture2D(Game.GraphicsDevice, width, height)

        Dim data As Color() = New Color(width * height - 1) {}
        For pixel As Integer = 0 To data.Count - 1
            data(pixel) = Color.White
        Next
        texture.SetData(data)

        Return texture
    End Function

    Public Sub UpdateTexture(ByRef texture As Texture2D, paint As Func(Of (Integer, Integer), Color))
        Dim data As Color() = New Color(texture.Width * texture.Height) {}
        For xPixel As Integer = 0 To texture.Width - 1
            For yPixel As Integer = 0 To texture.Height - 1
                data(xPixel + yPixel * texture.Width) = paint((xPixel, yPixel))
            Next
        Next
        texture.SetData(data)
    End Sub
End Class
