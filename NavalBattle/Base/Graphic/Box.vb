Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' Box.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Caixa de sprite possui 9 sprites que a compõe
''' LeftTop   | Top    | RightTop
''' Left      | Mid    | Right
''' LeftBottom| Bottom | RightBottom
''' 
''' Os intermediarios são repetidos para preencher os espaços vazios
''' Intermediarios:
'''             Top
'''             Left
'''             Mid
'''             Bottom
''' </summary>
Public Class Box
    Implements IDrawable
    Implements ITransform
    Implements IUpdate

    Private _TopLeft As Sprite
    Private _TopRight As Sprite
    Private _BottomLeft As Sprite
    Private _BottomRight As Sprite
    Private _Left As Sprite
    Private _Right As Sprite
    Private _Top As Sprite
    Private _Bottom As Sprite
    Private _Mid As Sprite
    Private _Size As Vector2

    ''' <summary>
    ''' Cria nova caixa que se ajusta ao tamanho Size sem escalar textura.
    ''' </summary>
    ''' <param name="size"> Tamanho da caixa </param>
    ''' <param name="topLeft"> Sprite TopLeft </param>
    ''' <param name="topRight"> Sprite TopRight </param>
    ''' <param name="bottomLeft"> Sprite BottomLeft </param>
    ''' <param name="bottomRight"> Sprite BottomRight </param>
    ''' <param name="left"> Sprite Left </param>
    ''' <param name="right"> Sprite Right</param>
    ''' <param name="top"> Sprite Top </param>
    ''' <param name="bottom"> Sprite Bottom </param>
    ''' <param name="mid"> Sprite Mid </param>
    Public Sub New(size As Vector2, topLeft As Sprite, topRight As Sprite, bottomLeft As Sprite, bottomRight As Sprite, left As Sprite, right As Sprite, top As Sprite, bottom As Sprite, mid As Sprite)
        Me.Size = size
        Me.TopLeft = topLeft
        Me.TopRight = topRight
        Me.BottomLeft = bottomLeft
        Me.BottomRight = bottomRight
        Me.Left = left
        Me.Right = right
        Me.Top = top
        Me.Bottom = bottom
        Me.Mid = mid
        Me.Position = Vector2.Zero
        Me.Scale = Vector2.One
        Me.Angle = 0F
        Me.Layer = 0L
        Me.LayerDepth = 0F
        Me.DrawEnable = True
    End Sub

    Public Sub New(size As Vector2, ByVal frame As Frame)
        Dim x As Integer = frame.source.X
        Dim y As Integer = frame.source.Y
        Dim width As Integer = frame.source.Width / 3
        Dim height As Integer = frame.source.Height / 3
        Dim texture As Texture2D = frame.texture

        Dim topLeft = New Sprite()
        topLeft.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x, y, width, height), Color.White, SpriteEffects.None)

        Dim top = New Sprite()
        top.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x + width, y, width, height), Color.White, SpriteEffects.None, True)

        Dim topRight = New Sprite()
        topRight.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x + 2 * width, y, width, height), Color.White, SpriteEffects.None)

        Dim bottomLeft = New Sprite()
        bottomLeft.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x, y + 2 * height, width, height), Color.White, SpriteEffects.None)

        Dim bottom = New Sprite()
        bottom.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x + width, y + 2 * height, width, height), Color.White, SpriteEffects.None, True)

        Dim bottomRight = New Sprite()
        bottomRight.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x + 2 * width, y + 2 * height, width, height), Color.White, SpriteEffects.None)


        Dim left = New Sprite()
        left.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x, y + height, width, height), Color.White, SpriteEffects.None, True)

        Dim middler = New Sprite()
        middler.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x + width, y + height, width, height), Color.White, SpriteEffects.None, True)

        Dim right = New Sprite()
        right.Frame = New Frame(texture, Vector2.Zero, New Rectangle(x + 2 * width, y + height, width, height), Color.White, SpriteEffects.None, True)

        Me.Size = size
        Me.TopLeft = topLeft
        Me.TopRight = topRight
        Me.BottomLeft = bottomLeft
        Me.BottomRight = bottomRight
        Me.Left = left
        Me.Right = right
        Me.Top = top
        Me.Bottom = bottom
        Me.Mid = middler
        Me.Position = Vector2.Zero
        Me.Scale = Vector2.One
        Me.Angle = 0F
        Me.Layer = 0L
        Me.LayerDepth = 0F
        Me.DrawEnable = True
    End Sub

    ''' <summary>
    ''' Desenha a caixa
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch objetivo </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, 0US)
    End Sub

    ''' <summary>
    ''' Desenha a caixa
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch objetivo </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, ByVal layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, layerDepthDelta)
    End Sub

    Private Sub TryUpdate(ByRef sprite As Sprite, ByRef gameTime As GameTime)
        If TypeOf sprite Is IUpdate Then
            Dim spriteUpdate As IUpdate = TryCast(sprite, IUpdate)
            If spriteUpdate.UpdateEnable Then
                spriteUpdate.Update(gameTime)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Update
    ''' Realiza update dos sprites internos se forem implementação de algum update e a flag UpdateEnable dos sprites estiverem ativados.
    ''' </summary>
    ''' <param name="gameTime"></param>
    Public Sub Update(gameTime As GameTime) Implements IUpdate.Update
        TryUpdate(TopLeft, gameTime)
        TryUpdate(Top, gameTime)
        TryUpdate(TopRight, gameTime)
        TryUpdate(Left, gameTime)
        TryUpdate(Mid, gameTime)
        TryUpdate(Right, gameTime)
        TryUpdate(BottomLeft, gameTime)
        TryUpdate(Bottom, gameTime)
        TryUpdate(BottomRight, gameTime)
    End Sub

    ''' <summary>
    ''' Desenha a caixa
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch objetivo </param>
    ''' <param name="transformDelta"> Transform adicional </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, transformDelta.Position, transformDelta.Scale, transformDelta.Angle, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Desenha a caixa
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch objetivo </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Angulo adicional </param>
    ''' <param name="layerDepthDelta"> LayerDepth adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw
        Dim position As Vector2 = Me.Position + positionDelta
        Dim scale As Vector2 = Me.Scale * scaleDelta
        Dim angle As Single = Me.Angle + angleDelta
        Dim layerDepth As UShort = Me.LayerDepth + layerDepthDelta

        Dim rotateMatrix As Matrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angle))
        Dim rotateMatrixInv As Matrix = Matrix.CreateRotationZ(MathHelper.ToRadians(-angle))

        Dim posTopLeft As Vector2 = (position + Vector2.Transform(New Vector2(-Size.X / 2, -Size.Y / 2) * scale, rotateMatrix))
        Dim posTopRight As Vector2 = (position + Vector2.Transform(New Vector2(Size.X / 2 - TopRight.Frame.source.Width, -Size.Y / 2) * scale, rotateMatrix))
        Dim posBottomLeft As Vector2 = (position + Vector2.Transform(New Vector2(-Size.X / 2, Size.Y / 2 - BottomLeft.Frame.source.Height) * scale, rotateMatrix))
        Dim posBottomRight As Vector2 = (position + Vector2.Transform(New Vector2(Size.X / 2 - BottomRight.Frame.source.Width, Size.Y / 2 - BottomLeft.Frame.source.Height) * scale, rotateMatrix))

        TopLeft.Frame.Draw(spriteBatch, posTopLeft, TopLeft.Scale, TopLeft.Angle + angle, TopLeft.LayerDepth + layerDepth)
        TopRight.Frame.Draw(spriteBatch, posTopRight, TopRight.Scale, TopRight.Angle + angle, TopRight.LayerDepth + layerDepth)
        BottomLeft.Frame.Draw(spriteBatch, posBottomLeft, BottomLeft.Scale, BottomLeft.Angle + angle, BottomLeft.LayerDepth + layerDepth)
        BottomRight.Frame.Draw(spriteBatch, posBottomRight, BottomRight.Scale, BottomRight.Angle + angle, BottomRight.LayerDepth + layerDepth)

        Dim posTop = posTopLeft + Vector2.Transform(New Vector2(TopLeft.Frame.source.Width, 0) * TopLeft.Scale, rotateMatrix)
        Dim posBottom = posBottomLeft + Vector2.Transform(New Vector2(BottomLeft.Frame.source.Width, 0) * BottomLeft.Scale, rotateMatrix)
        Dim posLeft = posTopLeft + Vector2.Transform(New Vector2(0, BottomLeft.Frame.source.Height) * BottomLeft.Scale, rotateMatrix)
        Dim posRight = posTopRight + Vector2.Transform(New Vector2(0, BottomLeft.Frame.source.Height) * BottomLeft.Scale, rotateMatrix)
        Dim posMid = posTopLeft + Vector2.Transform(New Vector2(TopLeft.Frame.source.Width, TopLeft.Frame.source.Height) * TopLeft.Scale, rotateMatrix)

        Dim scaleTop As Vector2 = (New Vector2(0, TopLeft.Frame.source.Height) / New Vector2(1, Top.Frame.source.Height)) + New Vector2(Vector2.Distance(posTopRight, posTop) / Top.Frame.source.Width, 0)
        Dim scaleBottom As Vector2 = (New Vector2(0, BottomLeft.Frame.source.Height) / New Vector2(1, Bottom.Frame.source.Height)) + New Vector2(Vector2.Distance(posBottomRight, posBottom) / Bottom.Frame.source.Width, 0)
        Dim scaleLeft As Vector2 = (New Vector2(TopLeft.Frame.source.Width, 0) / New Vector2(Left.Frame.source.Width, 1)) + New Vector2(0, Vector2.Distance(posBottomLeft, posLeft) / Left.Frame.source.Height)
        Dim scaleRight As Vector2 = (New Vector2(TopRight.Frame.source.Width, 0) / New Vector2(Right.Frame.source.Width, 1)) + New Vector2(0, Vector2.Distance(posBottomRight, posRight) / Right.Frame.source.Height)

        Dim scaleMid As Vector2 = New Vector2(0, (posBottomRight.Y - posRight.Y) / Right.Frame.source.Height) + New Vector2(Vector2.Distance(posTopRight, posTop) / Top.Frame.source.Width, 0)

        Top.Frame.Draw(spriteBatch, posTop, scaleTop, Top.Angle + angle, Top.LayerDepth + layerDepth)
        Bottom.Frame.Draw(spriteBatch, posBottom, scaleBottom, Bottom.Angle + angle, Bottom.LayerDepth + layerDepth)
        Left.Frame.Draw(spriteBatch, posLeft, scaleLeft, Left.Angle + angle, Left.LayerDepth + layerDepth)
        Right.Frame.Draw(spriteBatch, posRight, scaleRight, Right.Angle + angle, Right.LayerDepth + layerDepth)
        Mid.Frame.Draw(spriteBatch, posMid, scaleMid, Top.Angle + angle, Top.LayerDepth + layerDepth)
    End Sub

    ''' <summary>
    ''' Tamanho da caixa
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As Vector2
        Get
            Return _Size
        End Get
        Set
            _Size = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite superior esquerdo
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property TopLeft As Sprite
        Get
            Return _TopLeft
        End Get
        Private Set
            _TopLeft = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite superior direito
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property TopRight As Sprite
        Get
            Return _TopRight
        End Get
        Private Set
            _TopRight = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite inferior esquerdo
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property BottomLeft As Sprite
        Get
            Return _BottomLeft
        End Get
        Private Set
            _BottomLeft = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite inferior direito
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property BottomRight As Sprite
        Get
            Return _BottomRight
        End Get
        Private Set
            _BottomRight = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite esquerdo
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property Left As Sprite
        Get
            Return _Left
        End Get
        Private Set
            _Left = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite direito
    ''' </summary>
    ''' <returns></returns>
    Public Property Right As Sprite
        Get
            Return _Right
        End Get
        Private Set
            _Right = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite superior
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property Top As Sprite
        Get
            Return _Top
        End Get
        Private Set
            _Top = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite inferior
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property Bottom As Sprite
        Get
            Return _Bottom
        End Get
        Private Set
            _Bottom = Value
        End Set
    End Property

    ''' <summary>
    ''' Sprite do meio
    ''' </summary>
    ''' <returns> Sprite </returns>
    Public Property Mid As Sprite
        Get
            Return _Mid
        End Get
        Private Set
            _Mid = Value
        End Set
    End Property

    Public Property DrawEnable As Boolean Implements IDrawable.DrawEnable

    Public Property Layer As Long Implements IDrawable.Layer

    Public Property Position As Vector2 Implements ITransform.Position

    Public Property Angle As Single Implements ITransform.Angle

    Public Property Scale As Vector2 Implements ITransform.Scale

    Public Property UpdateEnable As Boolean Implements IUpdate.UpdateEnable

    Public Property LayerDepth As UShort Implements IDrawable.LayerDetph

End Class
