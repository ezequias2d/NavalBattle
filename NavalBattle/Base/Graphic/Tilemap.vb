Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Tilemap.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Tabela de telhas de sprites
''' </summary>
''' <typeparam name="Key"> Chave identificadora de Sprite</typeparam>
Public Class Tilemap(Of Key)
    Implements IDrawable
    Implements ITransform
    Implements IUpdate

    Private _DrawEnable As Boolean
    Private _Layer As Long
    Private _Position As Vector2
    Private _Angle As Single
    Private _Scale As Vector2
    Private _Sprites As IDictionary(Of Key, Sprite)
    Private _Map As Key(,)
    Private _Interval As Vector2

    Private width As Integer
    Private height As Integer
    Private _LayerDepth As UShort
    Private _UpdateEnable As Boolean

    ''' <summary>
    ''' Cria nova tabela de telhas de sprite
    ''' </summary>
    ''' <param name="x"> Número de colunas </param>
    ''' <param name="y"> Número de linhas </param>
    Public Sub New(ByVal x As Integer, ByVal y As Integer)
        Me.New(x, y, Nothing)
    End Sub

    ''' <summary>
    ''' Cria nova tabela de telhas de sprite com um dicionario que indexa Sprite com a Key
    ''' </summary>
    ''' <param name="x"> Número de colunas </param>
    ''' <param name="y"> Número de linhas </param>
    ''' <param name="sprites"> Dicionário de Sprites </param>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByRef sprites As IDictionary(Of Key, Sprite))
        Position = New Vector2(0F, 0F)
        Scale = New Vector2(1.0F, 1.0F)
        Angle = 0
        DrawEnable = True
        Interval = New Vector2(16.0F, 16.0F)

        _Map = New Key(x, y) {}
        width = x
        height = y
        If sprites Is Nothing Then
            Me.Sprites = New Dictionary(Of Key, Sprite)
        Else
            Me.Sprites = sprites
        End If

        LayerDepth = 0F
        Me.UpdateEnable = True
    End Sub

    ''' <summary>
    ''' Troca sprites de posição
    ''' </summary>
    ''' <param name="x1"> Posição X1(Coluna)</param>
    ''' <param name="y1"> Posição Y1(Linha) </param>
    ''' <param name="x2"> Posição X2(Coluna) </param>
    ''' <param name="y2"> Posição Y2(Linha) </param>
    Public Sub SwapIndex(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        Dim c As Key = _Map(x1, y1)
        _Map(x1, y1) = _Map(x2, y2)
        _Map(y2, y2) = c
    End Sub

    ''' <summary>
    ''' Seta sprite(por chave/key) na posição
    ''' </summary>
    ''' <param name="x"> Posição X(Coluna)</param>
    ''' <param name="y"> Posição Y(Linha)</param>
    ''' <param name="value"> Chave do sprite </param>
    Public Sub SetIndex(ByVal x As Integer, ByVal y As Integer, ByVal value As Key)
        _Map(x, y) = value
    End Sub

    ''' <summary>
    ''' Pega o objeto do mapa
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMap() As Key(,)
        Return _Map
    End Function

    ''' <summary>
    ''' Executa Update dos sprites, do dicionário de sprites, que implementão IUpdate se estiver com flag ativa(UpdateEnable)
    ''' </summary>
    ''' <param name="gameTime"> Tempo de jogo </param>
    Public Sub Update(gameTime As GameTime) Implements IUpdate.Update
        For Each Sprite In Sprites.Values
            If TypeOf Sprite Is IUpdate Then
                Dim spriteUpdate As IUpdate = TryCast(Sprite, IUpdate)
                If spriteUpdate.UpdateEnable Then
                    spriteUpdate.Update(gameTime)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Desenha todos os Sprites da tabela separados por Interval de distancia
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, ByVal layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Desenha todos os Sprites da tabela separados por Interval de distancia
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, 0US)
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Desenha todos os Sprites da tabela separados por Interval de distancia
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="transformDelta"> Transform adicional </param>    
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, transformDelta.Position, transformDelta.Scale, transformDelta.Angle, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Função de desenho
    ''' Desenha todos os Sprites da tabela separados por Interval de distancia
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch para desenhar </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Ângulo adicional </param>
    ''' <param name="layerDepthDelta"> Camada adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw
        Dim pos As Vector2 = New Vector2()
        Dim translate As Vector2 = New Vector2(width / 2)
        Dim sprite As Sprite
        Dim rotateMatrix As Matrix = Matrix.CreateRotationZ(MathHelper.ToRadians(Angle + angleDelta))
        For i As Integer = 0 To width - 1
            For j As Integer = 0 To height - 1
                sprite = Sprites(_Map(i, j))
                pos = Position + positionDelta + Vector2.Transform(New Vector2((i - (width / 2)) * Interval.X * Scale.X * scaleDelta.X, (j - (height / 2)) * Interval.Y * Scale.Y * scaleDelta.Y), rotateMatrix)
                If sprite.DrawEnable Then
                    sprite.Frame.Draw(spriteBatch, pos, sprite.Scale * Scale, sprite.Angle + Angle, sprite.LayerDepth + LayerDepth + layerDepthDelta)
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Camada de sobreposição de sprites
    ''' </summary>
    ''' <returns> Camada do sprite </returns>
    Public Property LayerDepth As UShort Implements IDrawable.LayerDetph
        Get
            Return _LayerDepth
        End Get
        Set
            _LayerDepth = Value
        End Set
    End Property

    ''' <summary>
    ''' Intervalo entre os elementos da tabela ao rederizar
    ''' </summary>
    ''' <returns></returns>
    Public Property Interval As Vector2
        Get
            Return _Interval
        End Get
        Set
            _Interval = Value
        End Set
    End Property

    ''' <summary>
    ''' Dicionario de sprites
    ''' </summary>
    ''' <returns> Sprites </returns>
    Public Property Sprites As IDictionary(Of Key, Sprite)
        Get
            Return _Sprites
        End Get
        Set
            If Value Is Nothing Then
                Throw New NullReferenceException("Framed não pode ser nulo!")
            End If
            _Sprites = Value
        End Set
    End Property

    ''' <summary>
    ''' Determina se está ativado para desenhar.
    ''' Se verdadeiro: desenha
    ''' Se falso: não desenha
    ''' </summary>
    ''' <returns> Se ativado o desenho </returns>
    Public Property DrawEnable As Boolean Implements IDrawable.DrawEnable
        Get
            Return _DrawEnable
        End Get
        Set(value As Boolean)
            _DrawEnable = value
        End Set
    End Property

    ''' <summary>
    ''' Camada de desenho que pertence.
    ''' Camadas podem ser ativadas ou desativadas da redenrização de uma camera especifica.
    ''' </summary>
    ''' <returns> Numero da camada </returns>
    Public Property Layer As Long Implements IDrawable.Layer
        Get
            Return _Layer
        End Get
        Set(value As Long)
            _Layer = value
        End Set
    End Property

    ''' <summary>
    ''' Posição no espaço virtual
    ''' </summary>
    ''' <returns> Posição </returns>
    Public Property Position As Vector2 Implements ITransform.Position
        Get
            Return _Position
        End Get
        Set(value As Vector2)
            _Position = value
        End Set
    End Property

    ''' <summary>
    ''' Angulo de rotação sobre o eixo Z
    ''' </summary>
    ''' <returns></returns>
    Public Property Angle As Single Implements ITransform.Angle
        Get
            Return _Angle
        End Get
        Set(value As Single)
            _Angle = value
        End Set
    End Property

    ''' <summary>
    ''' Escala no espaço virtual
    ''' </summary>
    ''' <returns> Escala no espaço virtual </returns>
    Public Property Scale As Vector2 Implements ITransform.Scale
        Get
            Return _Scale
        End Get
        Set(value As Vector2)
            _Scale = value
        End Set
    End Property

    ''' <summary>
    ''' Determina se está ativado o Update
    ''' </summary>
    ''' <returns> Se update está ativo </returns>
    Public Property UpdateEnable As Boolean Implements IUpdate.UpdateEnable
        Get
            Return _UpdateEnable
        End Get
        Set
            _UpdateEnable = Value
        End Set
    End Property
End Class
