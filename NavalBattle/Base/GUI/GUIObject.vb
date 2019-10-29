Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' GUIObject.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' Objeto de GUI
''' 
''' Classe abstrata = MustInherit
''' </summary>
Public MustInherit Class GUIObject
    Implements ITransform
    Implements IDrawable
    Implements IUpdate

    ''' <summary>
    ''' Construtor de GUIObject
    ''' </summary>
    ''' <param name="index"> Índice</param>
    ''' <param name="indexX"> Índice X </param>
    ''' <param name="indexY"> Índice Y </param>
    ''' <param name="position"> Posição no espaço </param>
    Public Sub New(index As Integer, indexX As Integer, indexY As Integer, position As Vector2)
        Me.Index = index
        Me.IndexX = indexX
        Me.IndexY = indexY
        Me.Position = position
        Me.GUIObjectEnable = True
        Me.Origin = Vector2.Zero
        Me.DrawEnable = True
        Me.Selected = False
        Me.LayerDetph = 1
        Me.Scale = Vector2.One
        Me.CountSpeed = 4.0F

        CursorMode = CursorMode.Arrow

        OnHorizontal = Nothing
        OnVertical = Nothing
        OnFire0 = Nothing
        OnFire1 = Nothing
        OnFire2 = Nothing
        OnJump = Nothing
        OnMouseX = Nothing
        OnMouseY = Nothing
        OnSubmit = Nothing
        OnCancel = AddressOf CancelFunction
        OnMouseScrollWheel = Nothing
        OnStart = Nothing
        OnOptions = Nothing
        UpdateEnable = True

        AxisModeSetting(Axis.Cancel) = AxisMode.Click
    End Sub

    ''' <summary>
    ''' Modo do cursor quando selecionado.
    ''' </summary>
    ''' <returns> Modo do cursor </returns>
    Public Property CursorMode As CursorMode

    ''' <summary>
    ''' Flag de GUIObject
    ''' Diz se é selecionável
    ''' </summary>
    ''' <returns> É selecionável </returns>
    Public Property GUIObjectEnable As Boolean

    ''' <summary>
    ''' Índice unidimensional
    ''' </summary>
    ''' <returns> Índice unidimensional </returns>
    Public Property Index As Integer

    ''' <summary>
    ''' Índice X(Coluna)
    ''' </summary>
    ''' <returns> Coluna </returns>
    Public Property IndexX As Integer

    ''' <summary>
    ''' Índice Y(Linha)
    ''' </summary>
    ''' <returns> Linha </returns>
    Public Property IndexY As Integer

    ''' <summary>
    ''' Origem para centralizar o cursor
    ''' </summary>
    ''' <returns> Origem </returns>
    Public Property Origin As Vector2

    ''' <summary>
    ''' Indica se está selecionado
    ''' </summary>
    ''' <returns></returns>
    Public Property Selected As Boolean

    ''' <summary>
    ''' Se objeto está focado
    ''' </summary>
    ''' <returns> Focado(true) ou não focado(false) </returns>
    Public Property Focus As Boolean
        Get
            Return _Focus
        End Get
        Set
            _Focus = Value
            If Value Then
                Me.Focused()
            Else
                Me.Unfocused()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Tipo de função de ação para entradas no Input
    ''' </summary>
    ''' <param name="context"> Contexto </param>
    ''' <param name="obj"> Objeto de GUI </param>
    ''' <param name="axisValue"> Valor do Axis </param>
    ''' <param name="axis"> Axis correspondente </param>
    Public Delegate Sub OnAxis(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)

    ''' <summary>
    ''' Tipo de função para determinar a cor do cursor.
    ''' </summary>
    ''' <param name="obj"> Objeto usado para calcular </param>
    ''' <returns> Cor do cursor </returns>
    Public Delegate Function CalculateColor(ByVal obj As GUIObject) As Color

    ''' <summary>
    ''' Variavel de função delegada que calcula a cor do cursor.
    ''' </summary>
    ''' <returns> Função delegada de CalculateColor </returns>
    Public Property CursorColor As CalculateColor

    ''' <summary>
    ''' Configuração do tipo de sensibilidade de comandos Axis em dicionario
    ''' 
    ''' AxisMode.Click = Chama função apenas no começo da ativação do Axis
    ''' AxisMode.Floating = Chamado enquanto ocorre a ativação do Axis
    ''' </summary>
    ''' <returns> Dicionario de configurações de Axis </returns>
    Public Property AxisModeSetting As IDictionary(Of Axis, AxisMode)
        Get
            If _AxisModeSetting Is Nothing Then
                _AxisModeSetting = New Dictionary(Of Axis, AxisMode)
            End If
            Return _AxisModeSetting
        End Get
        Set
            _AxisModeSetting = Value
        End Set
    End Property

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.Horizontal é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnHorizontal As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.Vertical é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnVertical As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.Fire0 é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Property OnFire0 As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.Fire1 é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnFire1 As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.Fire2 é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnFire2 As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.Jump é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnJump As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o  valor de Axis.MouseX é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnMouseX As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o valor de Axis.MouseY é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnMouseY As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o valor de Axis.Submit é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnSubmit As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o valor de Axis.Cancel é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnCancel As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o valor de Axis.MouseScrollWheel é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnMouseScrollWheel As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o valor de Axis.Start é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnStart As OnAxis

    ''' <summary>
    ''' Função que é invocada quando o valor de Axis.Options é diferente de 0
    ''' </summary>
    ''' <returns> Função </returns>
    Public Property OnOptions As OnAxis

    ''' <summary>
    ''' Pai do objeto
    ''' </summary>
    ''' <returns> Pai </returns>
    Public Property Parent As GUIObject

    Public Property UpdateEnable As Boolean Implements IUpdate.UpdateEnable

    Public Property DrawEnable As Boolean Implements IDrawable.DrawEnable

    Public Property Layer As Long Implements IDrawable.Layer

    Public Property LayerDetph As UShort Implements IDrawable.LayerDetph

    Public Property Position As Vector2 Implements ITransform.Position

    ''' <summary>
    ''' Posição transladada
    ''' 
    ''' Calcula a posição transladada a parti da soma das posições da posição relativa atual com a posição transladada do pai.
    ''' (Calculo de forma recursiva)
    ''' </summary>
    ''' <returns> Posição transladada atual </returns>
    Public ReadOnly Property PositionTranslated As Vector2
        Get
            Dim position As Vector2 = Me.Position
            If Parent IsNot Nothing Then
                position += Parent.PositionTranslated
            End If
            Return position
        End Get
    End Property

    Public Property Angle As Single Implements ITransform.Angle

    Public Property Scale As Vector2 Implements ITransform.Scale

    Public Property CountSpeed As Single
    ''' <summary>
    ''' Contador para invocar a função do Input
    ''' </summary>
    Private countInvoke As Single

    ''' <summary>
    ''' Axis atual contado por countInvoke
    ''' </summary>
    Private invokeCurrent As Axis

    Private _Focus As Boolean
    Private _AxisModeSetting As IDictionary(Of Axis, AxisMode)

    ''' <summary>
    ''' Chamado quando focado
    ''' </summary>
    Public Overridable Sub Focused()
        ' Desfoca automaticamente após ser focado
        ScreenManager.Instance.Current.GUIController.CurrentContext.Refocus()
    End Sub

    ''' <summary>
    ''' Chamado quando desfocado
    ''' </summary>
    Public Overridable Sub Unfocused()

    End Sub

    ''' <summary>
    ''' Chamado quando objeto é selecionado
    ''' </summary>
    Public Overridable Sub SelectedChange()

    End Sub

    ''' <summary>
    ''' Checa e chama todos os disparadores de Input.Axis
    ''' </summary>
    ''' <param name="gameTime"> Tempo de jogo </param>
    ''' <returns> Se foi disparado. </returns>
    Public Function InvokeAxisFulled(gameTime As GameTime) As Boolean
        Dim output As Boolean = False
        If OnHorizontal IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Horizontal, Me, OnHorizontal)
        End If

        If OnVertical IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Vertical, Me, OnVertical)
        End If

        If OnFire0 IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Fire0, Me, OnFire0)
        End If

        If OnFire1 IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Fire1, Me, OnFire1)
        End If

        If OnFire2 IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Fire2, Me, OnFire2)
        End If

        If OnJump IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Jump, Me, OnJump)
        End If

        If OnMouseX IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.MouseX, Me, OnMouseX)
        End If

        If OnMouseY IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.MouseY, Me, OnMouseY)
        End If

        If OnSubmit IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Submit, Me, OnSubmit)
        End If

        If OnCancel IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Cancel, Me, OnCancel)
        End If

        If OnMouseScrollWheel IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.MouseScrollWheel, Me, OnMouseScrollWheel)
        End If

        If OnStart IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Start, Me, OnStart)
        End If

        If OnOptions IsNot Nothing Then
            output = output Or InvokeAxis(gameTime, Axis.Options, Me, OnOptions)
        End If

        Return output
    End Function

    ''' <summary>
    ''' Update de GUIObject
    ''' </summary>
    ''' <param name="gameTime"> Tempo de jogo </param>
    Public Overridable Sub Update(gameTime As GameTime) Implements IUpdate.Update
        If Not Selected Or Not Focus Then
            ' Se não focado, não precisa atualizar.
            Return
        End If
        InvokeAxisFulled(gameTime)
    End Sub

    ''' <summary>
    ''' Função disparada em Axis.Cancel por padrão
    ''' </summary>
    ''' <param name="context"> Contexto do objeto disparado </param>
    ''' <param name="obj"> Objeto que disparou </param>
    ''' <param name="axisValue"> Valor do Axis quando disparado </param>
    ''' <param name="axis"> Axis </param>
    Protected Sub CancelFunction(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        context.GetContext().Refocus()
    End Sub

    ''' <summary>
    ''' Pega GUIContext associado(não necessariamente ele é o pai)
    ''' </summary>
    ''' <returns></returns>
    Public Function GetContext() As GUIContext
        Dim context As GUIContext
        If TypeOf Me Is GUIContext And Me.Parent Is Nothing Then
            ' Se não possui pai e é um GUIContext, considera a si proprio como o GUIContext associado.
            context = Me
        Else
            Dim possibleParentContext As GUIObject = Me.Parent
            While TypeOf possibleParentContext IsNot GUIContext
                ' equanto não encontra um pai que não é GUIContext objeto, continua
                If possibleParentContext.Parent Is Nothing Then
                    ' emite um exceção de não encontrado por não possuir mais pais
                    Throw New NullReferenceException("Não foi encontrado contexto do objeto atual.")
                End If
                ' sobe na hierarquia
                possibleParentContext = possibleParentContext.Parent
            End While
            ' cast dinamico entre tipos
            context = TryCast(possibleParentContext, GUIContext)
        End If
        Return context
    End Function

    ''' <summary>
    ''' Invoca uma função do tipo OnAxis quando satisfaz condições:
    '''     Quando o AxisMode é Floating:
    '''         - Input.ReadAxis diferente de 0
    '''         - countInvoke deve passar de 1.0F
    '''         - invokeCurrent deve corresponder ao Axis
    '''         Quando não comprido:
    '''             - invokeCurrent é mudado para Axis(se este for direferente)
    '''             - countInvoke é zerado(se houve mudança em invokeCurrent), ou incrementado por elapse * CountSpeed
    '''             - retorna falso indicando que não foi disparado
    '''         Quando comprido:
    '''             - zera invokeCurrent
    '''             - dispara o metodo associado
    '''             - retorna verdadeiro indicando que foi disparado
    '''     Quando o AxisMode é Click:
    '''         - Input.ReadAxisClick diferente de 0
    '''         Quando não comprido:
    '''             - retorna falso indicando que não foi disparado
    '''         Quando comprido:
    '''             - dispara o metodo associado
    '''             - retorna true indicando que foi disparado
    '''         
    ''' </summary>
    ''' <param name="elapse"> Tempo que passou/elapsado </param>
    ''' <param name="axis"> Axis </param>
    ''' <param name="current"> GUIObject do possivel disparo </param>
    ''' <param name="func"> Função de disparo </param>
    ''' <returns> Se houve o disparo da função </returns>
    Protected Function InvokeAxis(elapse As Single, axis As Axis, current As GUIObject, func As GUIObject.OnAxis) As Boolean
        Dim context As GUIContext

        Try
            context = GetContext()
        Catch ex As NullReferenceException
            Throw New NullReferenceException("O invoke da função: " + func.Method.Name + " não pode ser invocado no objeto de GUI: " + current.GetType().FullName + " pois não foi encontrado um contexto pai.")
        End Try

        Dim axisValue As Single = Input.ReadAxis(axis)
        If axisValue <> 0 And (Not AxisModeSetting.ContainsKey(axis) OrElse AxisModeSetting(axis) = AxisMode.Floating) Then
            If invokeCurrent = axis Then
                countInvoke += elapse * 4.0F
            Else
                invokeCurrent = axis
                countInvoke = 0
            End If

            If countInvoke > 1 Then
                countInvoke = 0
                func.Invoke(context, current, axisValue, axis)
                Return True
            End If
        ElseIf Input.ReadAxisClick(axis) <> 0 Then
            countInvoke = 0
            invokeCurrent = axis
            func.Invoke(context, current, axisValue, axis)
            Return True
        End If
        Return False
    End Function

    Protected Function InvokeAxis(gameTime As GameTime, axis As Axis, current As GUIObject, func As GUIObject.OnAxis) As Boolean
        Return InvokeAxis(gameTime.ElapsedGameTime.TotalSeconds, axis, current, func)
    End Function

    Public Overridable Sub Draw(ByRef spriteBatch As SpriteBatch) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, 0US)
    End Sub

    Public Overridable Sub Draw(ByRef spriteBatch As SpriteBatch, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, Vector2.Zero, Vector2.One, 0F, layerDepthDelta)
    End Sub

    Public Overridable Sub Draw(ByRef spriteBatch As SpriteBatch, transformDelta As ITransform, layerDepthDelta As UShort) Implements IDrawable.Draw
        Draw(spriteBatch, transformDelta.Position, transformDelta.Scale, transformDelta.Angle, layerDepthDelta)
    End Sub

    Public MustOverride Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort) Implements IDrawable.Draw

End Class
