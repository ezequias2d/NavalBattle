Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Content
Imports Microsoft.Xna.Framework.Graphics

''' <summary>
''' Gerenciador de tela
''' </summary>
Public Class ScreenManager
    Private Shared _Instance As ScreenManager
    Private Shared _Dimensions As Vector2
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
    Public Property Game As Microsoft.Xna.Framework.Game

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
            _Dimensions = Value
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
    ''' Cria novo ScreenManager de relolução 448x512
    ''' </summary>
    Private Sub New()
        Dimensions = New Vector2(448, 512)
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
        If newScene Is Nothing Then
            Return
        End If

        If Not Current Is Nothing Then
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
End Class
