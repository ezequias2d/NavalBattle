﻿Imports Microsoft.Xna.Framework
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
        stackContext.Push(CurrentContext)
        CurrentContext.Focus = False
        CurrentContext.UpdateEnable = False
        _CurrentContext = context
        CurrentContext.Focus = True
        Input.Instance.Update(Nothing)
    End Sub

    ''' <summary>
    ''' Retorna ao ultimo GUIContext inserido na pilha
    ''' </summary>
    Public Sub GoBack()
        If stackContext.Count > 0 Then
            CurrentContext.Focus = False
            CurrentContext.UpdateEnable = False
            CurrentContext.ResetFocus()
            _CurrentContext = stackContext.Pop()
            CurrentContext.Focus = True
        End If
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
End Class
