Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' GUIController.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Controla a navegação da GUI entre GUIContext
''' </summary>
Public Class GUIController
    Implements IUpdate
    Implements IDrawable

    Private ReadOnly _MainContext As GUIContext
    Private Shared _Texture As Texture2D
    Private _CurrentContext As GUIContext
    Private _LockContext As Boolean

    ''' <summary>
    ''' Flag de bloqueio
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LockContext As Boolean
        Get
            Return _LockContext
        End Get
    End Property

    ''' <summary>
    ''' Contexto principal
    ''' </summary>
    ''' <returns> Contexto principal </returns>
    Public ReadOnly Property MainContext As GUIContext
        Get
            Return _MainContext
        End Get
    End Property

    ''' <summary>
    ''' Contexto atual
    ''' </summary>
    ''' <returns> Contexto atual </returns>
    Public ReadOnly Property CurrentContext As GUIContext
        Get
            Return _CurrentContext
        End Get
    End Property

    Public Property UpdateEnable As Boolean Implements IUpdate.UpdateEnable

    Public Property DrawEnable As Boolean Implements IDrawable.DrawEnable

    Public Property Layer As Long Implements IDrawable.Layer

    Public Property LayerDetph As UShort Implements IDrawable.LayerDetph

    ''' <summary>
    ''' Textura de controles da GUI
    ''' </summary>
    ''' <returns> Textura de controles da GUI</returns>
    Public Shared ReadOnly Property Texture As Texture2D
        Get
            If _Texture Is Nothing Then
                _Texture = ScreenManager.Instance.Content.Load(Of Texture2D)("gui")
            End If
            Return _Texture
        End Get
    End Property

    ''' <summary>
    ''' Stack de contextos
    ''' </summary>
    Private stackContext As Stack(Of GUIContext)

    ''' <summary>
    ''' Cria novo contexto com dimenção especifica.
    ''' </summary>
    ''' <param name="dimensions"></param>
    Public Sub New(dimensions As Vector2)
        stackContext = New Stack(Of GUIContext)()
        _MainContext = New GUIContext(dimensions)
        _CurrentContext = MainContext
        MainContext.Focus = True
        Me.DrawEnable = True
        Me.UpdateEnable = True
        Me._LockContext = False
    End Sub

    ''' <summary>
    ''' Cria um Frame de tamanho 1x1 branco.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function CreateSimpleFrame() As Frame
        Dim texture As Texture2D = New Texture2D(ScreenManager.Instance.Game.GraphicsDevice, 1, 1)
        Dim colors As Color() = New Color() {Color.White}
        texture.SetData(colors)
        Return New Frame(texture, New Rectangle(0, 0, 1, 1), True)
    End Function

    ''' <summary>
    ''' Muda de GUIContext e coloca o GUIContext atual na pilha de GUIContext.
    ''' </summary>
    ''' <param name="context"></param>
    Public Sub ChangeContext(ByRef context As GUIContext)
        If Not LockContext Then
            stackContext.Push(CurrentContext)
            CurrentContext.Focus = False
            CurrentContext.UpdateEnable = False
            _CurrentContext = context
            CurrentContext.Focus = True
            Input.Instance.Update(Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Retorna ao ultimo GUIContext inserido na pilha
    ''' </summary>
    Public Sub GoBack()
        If stackContext.Count > 0 AndAlso Not LockContext Then
            CurrentContext.Focus = False
            CurrentContext.UpdateEnable = False
            CurrentContext.ResetFocus()
            _CurrentContext = stackContext.Pop()
            CurrentContext.Focus = True
        End If
    End Sub

    ''' <summary>
    ''' Limpa pilha de GUIContext
    ''' </summary>
    Public Sub ClearStack()
        stackContext.Clear()
        _CurrentContext = MainContext
    End Sub

    ''' <summary>
    ''' Update do GUIContext.
    ''' Chamado a cada frame quando ativado.
    ''' </summary>
    ''' <param name="gameTime"> Tempo de jogo </param>
    Public Sub Update(gameTime As GameTime) Implements IUpdate.Update
        If CurrentContext.UpdateEnable Then
            CurrentContext.Update(gameTime)
        Else
            CurrentContext.Load()
        End If
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, 0US)
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch com camada adicional de profundidade de sprite
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="layerDepthDelta"> Camada adicional de profundidade de sprite </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch com transformação adicional e camada de profundidade de sprite adicional.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="transformDelta"> Transformação adicional </param>
    ''' <param name="layerDepthDelta"> Camada de profundidade adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, transformDelta.Position, transformDelta.Scale, transformDelta.Angle, layerDepthDelta)
    End Sub

    ''' <summary>
    ''' Desenha no SpriteBatch com posição, escala e ângulo adicional e com camada de profundidade de sprite adicional.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Ângulo adicional </param>
    ''' <param name="layerDepthDelta"> Camada de profundidade de sprite adicional </param>
    Public Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw
        If CurrentContext.DrawEnable Then
            CurrentContext.Draw(spriteBatch, positionDelta, scaleDelta, angleDelta, layerDepthDelta)
        End If
    End Sub

    Public Sub LockChangeContext()
        _LockContext = True
    End Sub

    Public Sub UnlockChangeContext()
        _LockContext = False
    End Sub

    Private Function CreateSprite8x8(x As Integer, y As Integer, color As Color) As Sprite
        Dim sprite As Sprite = New Sprite()
        sprite.Frame = New Frame(Texture, Vector2.One * 4, New Rectangle(x, y, 8, 8), color, SpriteEffects.None)
        Return sprite
    End Function

    Private Function CreateSprite16x16(x As Integer, y As Integer, color As Color) As Sprite
        Dim sprite As Sprite = New Sprite()
        sprite.Frame = New Frame(Texture, Vector2.One * 4, New Rectangle(x, y, 16, 16), color, SpriteEffects.None)
        Return sprite
    End Function

    Public Function CreateFireSprite(color As Color) As Sprite
        Return CreateSprite8x8(24, 40, color)
    End Function

    Public Function CreateHorizontalSprite(color As Color) As Sprite
        Return CreateSprite8x8(40, 32, color)
    End Function

    Public Function CreateVerticalSprite(color As Color) As Sprite
        Return CreateSprite8x8(32, 40, color)
    End Function

    Public Function CreateOrbSprite(color As Color) As Sprite
        Return CreateSprite8x8(40, 24, color)
    End Function

    Public Function CreateUpSprite(color As Color) As Sprite
        Return CreateSprite8x8(24, 24, color)
    End Function

    Public Function CreateDownSprite(color As Color) As Sprite
        Return CreateSprite8x8(32, 24, color)
    End Function

    Public Function CreateLeftSprite(color As Color) As Sprite
        Return CreateSprite8x8(32, 32, color)
    End Function

    Public Function CreateRightSprite(color As Color) As Sprite
        Return CreateSprite8x8(24, 32, color)
    End Function

    Public Function CreateLittleOrbSprite(color As Color) As Sprite
        Return CreateSprite8x8(24, 48, color)
    End Function

    Public Function CreateAnalogicSprite(color As Color) As Sprite
        Return CreateSprite16x16(48, 24, color)
    End Function
End Class
