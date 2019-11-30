Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Camera.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Camera 2D
''' Reponsavel por movimentar o espaço, delimitar area de desenho e desenhar todos os objetos na lista de
''' desenhaveis (<see cref="IDrawable"/>, <see cref="Camera.Drawings"/>)
''' </summary>
Public Class Camera
    Implements ITransform

    Private Shared _Drawings As List(Of IDrawable)

    ''' <summary>
    ''' Objetos para desenhar
    ''' </summary>
    ''' <returns> Objetos para desenhar </returns>
    Public Shared ReadOnly Property Drawings As List(Of IDrawable)
        Get
            If _Drawings Is Nothing Then
                _Drawings = New List(Of IDrawable)
            End If

            Return _Drawings
        End Get
    End Property

    ''' <summary>
    ''' Dimenção da janela/tela do jogo
    ''' </summary>
    ''' <returns> Dimenção </returns>
    Public ReadOnly Property Dimensions As Vector2
        Get
            Return ScreenManager.Instance.Dimensions
        End Get
    End Property

    ''' <summary>
    ''' Dimenção virtual do jogo
    ''' </summary>
    ''' <returns> Dimenção virtual </returns>
    Public Property InternalDimensions As Vector2
        Get
            Return _InternalDimensions
        End Get
        Set
            _InternalDimensions = Value
        End Set
    End Property

    ''' <summary>
    ''' Posição da Camera no jogo
    ''' </summary>
    ''' <returns> Posição da Camera </returns>
    Public Property Position As Vector2 Implements ITransform.Position
        Get
            Return _Position
        End Get
        Set(value As Vector2)
            _Position = value
        End Set
    End Property

    ''' <summary>
    ''' Posição Z da Camera.
    ''' 
    ''' Quanto menor, maior as coisas parecem.
    ''' Quanto maior, menor as coisas parecem.
    ''' Inversamente proporcinal.
    ''' </summary>
    ''' <returns></returns>
    Public Property ZPosition As Single
        Get
            Return _ZPosition
        End Get
        Set
            _ZPosition = Value
        End Set
    End Property

    ''' <summary>
    ''' Ângulo da Camera
    ''' </summary>
    ''' <returns> Ângulo da Camera </returns>
    Public Property Angle As Single Implements ITransform.Angle
        Get
            Return _Angle
        End Get
        Set(value As Single)
            _Angle = value
        End Set
    End Property

    ''' <summary>
    ''' Escala da Camera, afeta diretamente o formato e tamanho das rederizações.
    ''' </summary>
    ''' <returns> Escala da Camera </returns>
    Public Property Scale As Vector2 Implements ITransform.Scale
        Get
            Return _Scale
        End Get
        Set(value As Vector2)
            _Scale = value
        End Set
    End Property

    ''' <summary>
    ''' Camadas ativas da Camera.
    ''' Objetos que estejam em camadas que não estejam na lista, não seram rederizados.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Layers As HashSet(Of Long)
        Get
            If _Layers Is Nothing Then
                _Layers = New HashSet(Of Long)
            End If
            Return _Layers
        End Get
    End Property

    ''' <summary>
    ''' Viewport que delimita a area de desenho
    ''' </summary>
    Public viewport As Viewport
    Public samplerState As SamplerState

    Private spriteBatch As SpriteBatch
    Private _InternalDimensions As Vector2
    Private _ZPosition As Single
    Private _Position As Vector2
    Private _Scale As Vector2
    Private _Angle As Single
    Private _Layers As HashSet(Of Long)

    ''' <summary>
    ''' Cria uma nova camera
    ''' </summary>
    ''' <param name="internalDimensions"> Dimenção virtual </param>
    Public Sub New(ByVal internalDimensions As Vector2, ByRef spriteBatch As SpriteBatch)
        Me.InternalDimensions = internalDimensions
        Me.spriteBatch = spriteBatch
        Me.samplerState = SamplerState.PointClamp
        Me.viewport = New Viewport(New Rectangle(0, 0, Dimensions.X, Dimensions.Y))
        Me.Scale = New Vector2(1.0F, 1.0F)
        Me.ZPosition = 1.0F
        Me.Layers.Add(0L)
    End Sub

    ''' <summary>
    ''' Inicializa o SpriteBatch
    ''' </summary>
    Private Sub BeginSpriteBatch()
        Dim scaleResolution As Single = 1.0F
        If Dimensions.X < Dimensions.Y Then
            scaleResolution = Dimensions.X / InternalDimensions.X
        Else
            scaleResolution = Dimensions.Y / InternalDimensions.Y
        End If

        If viewport.Width <> Dimensions.X OrElse viewport.Height <> Dimensions.Y Then
            viewport = New Viewport(New Rectangle(0, 0, Dimensions.X, Dimensions.Y))
        End If

        Dim transform As Matrix = Matrix.CreateTranslation(New Vector3(-Position.X, -Position.Y, 0F)) *
            Matrix.CreateRotationZ(MathHelper.ToRadians(Angle)) *
            Matrix.CreateScale(New Vector3(Scale.X, Scale.Y, 1.0F) * scaleResolution / ZPosition) *
            Matrix.CreateTranslation(New Vector3(viewport.Width * 0.5F, viewport.Height * 0.5F, 0F))

        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState, Nothing, Nothing, Nothing, transform)
    End Sub

    ''' <summary>
    ''' Desenha objetos em desenhaveis na camera.
    ''' </summary>
    ''' <remarks>
    ''' <seealso cref="IDrawable"/>
    ''' </remarks>
    Public Sub Draw()
        BeginSpriteBatch()
        spriteBatch.GraphicsDevice.Viewport = viewport

        Dim drawingsList As List(Of IDrawable) = Drawings

        For Each drawable As IDrawable In drawingsList
            If drawable.DrawEnable AndAlso Me.Layers.Contains(drawable.Layer) Then
                drawable.Draw(spriteBatch)
            End If
        Next

        spriteBatch.End()
    End Sub
End Class
