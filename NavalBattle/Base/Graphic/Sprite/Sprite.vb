Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Sprite.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Sprite
''' </summary>
Public Class Sprite
    Implements IDrawable
    Implements ITransform

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
    ''' Camada de sobreposição de sprites
    ''' </summary>
    ''' <returns> Camada do sprite </returns>
    Public Property LayerDepth As UShort Implements IDrawable.LayerDetph
        Get
            Return _layerDepth
        End Get
        Set(value As UShort)
            _layerDepth = value
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
    ''' Frame do sprite
    ''' </summary>
    ''' <returns> Frame </returns>
    Public Property Frame As Frame
        Get
            Return _Frame
        End Get
        Set
            _Frame = Value
        End Set
    End Property

    ''' <summary>
    ''' Camada de desenho que pertence.
    ''' Camadas podem ser ativadas ou desativadas da redenrização de uma camera especifica.
    ''' </summary>
    ''' <returns> Numero da camada </returns>
    Public Property Layer As Long Implements IDrawable.Layer
        Get
            Return Layer
        End Get
        Set(value As Long)
            _Layer = value
        End Set
    End Property

    Private _DrawEnable As Boolean
    Private _Position As Vector2
    Private _Angle As Single
    Private _layerDepth As UShort
    Private _Scale As Vector2
    Private _Layer As Long
    Private _Frame As Frame

    ''' <summary>
    ''' Cria novo Sprite
    ''' </summary>
    ''' <param name="position"> Posição </param>
    Public Sub New(position As Vector2)
        Me.Position = position
        Me.DrawEnable = True
        Me.Angle = 0F
        Me.LayerDepth = 0F
        Me.Scale = New Vector2(1.0F, 1.0F)
        Me.Layer = 0
    End Sub

    ''' <summary>
    ''' Cria novo Sprite em (0.0F, 0.0F)
    ''' </summary>
    Public Sub New()
        Me.New(New Vector2(0F, 0F))
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Frame.Draw(spriteBatch, Me, LayerDepth)
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="layerDepthDelta"> LAyerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, layerDepthDelta As UShort) Implements IDrawable.Draw
        Frame.Draw(spriteBatch, Me, LayerDepth + layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="transformDelta"> Transformo adicional </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Frame.Draw(spriteBatch, Position + transformDelta.Position, Scale + transformDelta.Scale, Angle + transformDelta.Angle, layerDepthDelta + LayerDepth)
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Ângulo adicional </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw
        Frame.Draw(spriteBatch, Position + positionDelta, Scale * scaleDelta, Angle + angleDelta, layerDepthDelta + LayerDepth)
    End Sub
End Class
