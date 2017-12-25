Imports System.Threading
Imports System.Windows.Controls.Primitives
Imports System.Windows.Threading

<TemplatePart(Name:=AutoCompleteTextBox.PartEditor, Type:=GetType(TextBox))> _
<TemplatePart(Name:=AutoCompleteTextBox.PartPopup, Type:=GetType(Popup))> _
<TemplatePart(Name:=AutoCompleteTextBox.PartSelector, Type:=GetType(Selector))> _
Public Class AutoCompleteTextBox
    Inherits Control

#Region "Fields"

    Public Const PartEditor As String = "PART_Editor"
    Public Const PartPopup As String = "PART_Popup"
    Public Const PartSelector As String = "PART_Selector"

    Public Shared ReadOnly DelayProperty As DependencyProperty = DependencyProperty.Register("Delay", GetType(Integer), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(200))
    Public Shared ReadOnly DisplayMemberProperty As DependencyProperty = DependencyProperty.Register("DisplayMember", GetType(String), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(String.Empty))
    Public Shared ReadOnly IconPlacementProperty As DependencyProperty = DependencyProperty.Register("IconPlacement", GetType(IconPlacement), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(IconPlacement.Left))
    Public Shared ReadOnly IconProperty As DependencyProperty = DependencyProperty.Register("Icon", GetType(Object), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(Nothing))
    Public Shared ReadOnly IconVisibilityProperty As DependencyProperty = DependencyProperty.Register("IconVisibility", GetType(Visibility), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(Visibility.Visible))
    Public Shared ReadOnly IsDropDownOpenProperty As DependencyProperty = DependencyProperty.Register("IsDropDownOpen", GetType(Boolean), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(False))
    Public Shared ReadOnly IsLoadingProperty As DependencyProperty = DependencyProperty.Register("IsLoading", GetType(Boolean), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(False))
    Public Shared ReadOnly IsReadOnlyProperty As DependencyProperty = DependencyProperty.Register("IsReadOnly", GetType(Boolean), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(False))
    Public Shared ReadOnly ItemTemplateProperty As DependencyProperty = DependencyProperty.Register("ItemTemplate", GetType(DataTemplate), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(Nothing))
    Public Shared ReadOnly LoadingContentProperty As DependencyProperty = DependencyProperty.Register("LoadingContent", GetType(Object), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(Nothing))
    Public Shared ReadOnly ProviderProperty As DependencyProperty = DependencyProperty.Register("Provider", GetType(ISuggestionProvider), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(Nothing))
    Public Shared ReadOnly SelectedItemProperty As DependencyProperty = DependencyProperty.Register("SelectedItem", GetType(Object), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(Nothing, AddressOf OnSelectedItemChanged))
    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(String.Empty))
    Public Shared ReadOnly WatermarkProperty As DependencyProperty = DependencyProperty.Register("Watermark", GetType(String), GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(String.Empty))

    Private _bindingEvaluator As BindingEvaluator
    Private _editor As TextBox
    Private _fetchTimer As DispatcherTimer
    Private _filter As String
    Private _isUpdatingText As Boolean
    Private _itemsSelector As Selector
    Private _popup As Popup
    Private _selectionAdapter As SelectionAdapter
    Private _suggestionsAdapter As SuggestionsAdapter

#End Region 'Fields

#Region "Constructors"

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(AutoCompleteTextBox), New FrameworkPropertyMetadata(GetType(AutoCompleteTextBox)))
    End Sub

#End Region 'Constructors

#Region "Properties"

    Public Property BindingEvaluator() As BindingEvaluator
        Get
            Return _bindingEvaluator
        End Get
        Set(ByVal value As BindingEvaluator)
            _bindingEvaluator = value
        End Set
    End Property

    Public Property Delay() As Integer
        Get
            Return GetValue(DelayProperty)
        End Get

        Set(ByVal value As Integer)
            SetValue(DelayProperty, value)
        End Set
    End Property

    Public Property DisplayMember() As String
        Get
            Return GetValue(DisplayMemberProperty)
        End Get

        Set(ByVal value As String)
            SetValue(DisplayMemberProperty, value)
        End Set
    End Property

    Public Property Editor() As TextBox
        Get
            Return _editor
        End Get
        Set(ByVal value As TextBox)
            _editor = value
        End Set
    End Property

    Public Property FetchTimer() As DispatcherTimer
        Get
            Return _fetchTimer
        End Get
        Set(ByVal value As DispatcherTimer)
            _fetchTimer = value
        End Set
    End Property

    Public Property Filter() As String
        Get
            Return _filter
        End Get
        Set(ByVal value As String)
            _filter = value
        End Set
    End Property

    Public Property Icon() As Object
        Get
            Return GetValue(IconProperty)
        End Get

        Set(ByVal value As Object)
            SetValue(IconProperty, value)
        End Set
    End Property

    Public Property IconPlacement() As IconPlacement
        Get
            Return GetValue(IconPlacementProperty)
        End Get

        Set(ByVal value As IconPlacement)
            SetValue(IconPlacementProperty, value)
        End Set
    End Property

    Public Property IconVisibility() As Visibility
        Get
            Return GetValue(IconVisibilityProperty)
        End Get

        Set(ByVal value As Visibility)
            SetValue(IconVisibilityProperty, value)
        End Set
    End Property

    Public Property IsDropDownOpen() As Boolean
        Get
            Return GetValue(IsDropDownOpenProperty)
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsDropDownOpenProperty, value)
        End Set
    End Property

    Public Property IsLoading() As Boolean
        Get
            Return GetValue(IsLoadingProperty)
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsLoadingProperty, value)
        End Set
    End Property

    Public Property IsReadOnly() As Boolean
        Get
            Return GetValue(IsReadOnlyProperty)
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsReadOnlyProperty, value)
        End Set
    End Property

    Public Property ItemsSelector() As Selector
        Get
            Return _itemsSelector
        End Get
        Set(ByVal value As Selector)
            _itemsSelector = value
        End Set
    End Property

    Public Property ItemTemplate() As DataTemplate
        Get
            Return GetValue(ItemTemplateProperty)
        End Get

        Set(ByVal value As DataTemplate)
            SetValue(ItemTemplateProperty, value)
        End Set
    End Property

    Public Property LoadingContent() As Object
        Get
            Return GetValue(LoadingContentProperty)
        End Get

        Set(ByVal value As Object)
            SetValue(LoadingContentProperty, value)
        End Set
    End Property

    Public Property Popup() As Popup
        Get
            Return _popup
        End Get
        Set(ByVal value As Popup)
            _popup = value
        End Set
    End Property

    Public Property Provider() As ISuggestionProvider
        Get
            Return GetValue(ProviderProperty)
        End Get

        Set(ByVal value As ISuggestionProvider)
            SetValue(ProviderProperty, value)
        End Set
    End Property

    Public Property SelectedItem() As Object
        Get
            Return GetValue(SelectedItemProperty)
        End Get

        Set(ByVal value As Object)
            SetValue(SelectedItemProperty, value)
        End Set
    End Property

    Public Property SelectionAdapter() As SelectionAdapter
        Get
            Return _selectionAdapter
        End Get
        Set(ByVal value As SelectionAdapter)
            _selectionAdapter = value
        End Set
    End Property

    Public Property Text() As String
        Get
            Return GetValue(TextProperty)
        End Get

        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set
    End Property

    Public Property Watermark() As String
        Get
            Return GetValue(WatermarkProperty)
        End Get

        Set(ByVal value As String)
            SetValue(WatermarkProperty, value)
        End Set
    End Property

#End Region 'Properties

#Region "Methods"

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()
        Editor = Template.FindName(PartEditor, Me)
        Popup = Template.FindName(PartPopup, Me)
        ItemsSelector = Template.FindName(PartSelector, Me)
        BindingEvaluator = New BindingEvaluator(New Binding(DisplayMember))

        If Editor IsNot Nothing Then
            AddHandler Editor.TextChanged, AddressOf OnEditorTextChanged
            AddHandler Editor.PreviewKeyDown, AddressOf OnEditorKeyDown
            AddHandler Editor.LostFocus, AddressOf OnEditorLostFocus

            If SelectedItem IsNot Nothing Then
                Editor.Text = BindingEvaluator.Evaluate(SelectedItem)
            End If

        End If

        If Popup IsNot Nothing Then
            Popup.StaysOpen = False
            AddHandler Popup.Opened, AddressOf OnPopupOpened
            AddHandler Popup.Closed, AddressOf OnPopupClosed
        End If
        If ItemsSelector IsNot Nothing Then
            SelectionAdapter = New SelectionAdapter(ItemsSelector)
            AddHandler SelectionAdapter.Commit, AddressOf OnSelectionAdapterCommit
            AddHandler SelectionAdapter.Cancel, AddressOf OnSelectionAdapterCancel
            AddHandler SelectionAdapter.SelectionChanged, AddressOf OnSelectionAdapterSelectionChanged
        End If
    End Sub

    Shared Sub OnSelectedItemChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim act As AutoCompleteTextBox
        act = TryCast(d, AutoCompleteTextBox)
        If act IsNot Nothing Then
            If act.Editor IsNot Nothing And Not act._isUpdatingText Then
                act._isUpdatingText = True
                act.Editor.Text = act.BindingEvaluator.Evaluate(e.NewValue)
                act._isUpdatingText = False
            End If
        End If
    End Sub

    Private Function GetDisplayText(ByVal dataItem As Object) As String
        If BindingEvaluator Is Nothing Then
            BindingEvaluator = New BindingEvaluator(New Binding(DisplayMember))
        End If
        If dataItem Is Nothing Then
            Return String.Empty
        End If
        If String.IsNullOrEmpty(DisplayMember) Then
            Return dataItem.ToString()
        End If
        Return BindingEvaluator.Evaluate(dataItem)
    End Function

    Private Sub OnEditorKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If SelectionAdapter IsNot Nothing Then
            SelectionAdapter.HandleKeyDown(e)
        End If
    End Sub

    Private Sub OnEditorLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If Not IsKeyboardFocusWithin Then
            IsDropDownOpen = False
        End If
    End Sub

    Private Sub OnEditorTextChanged(ByVal sender As Object, ByVal e As TextChangedEventArgs)
        If _isUpdatingText Then Return
        If FetchTimer Is Nothing Then
            FetchTimer = New DispatcherTimer
            FetchTimer.Interval = TimeSpan.FromMilliseconds(Delay)
            AddHandler FetchTimer.Tick, AddressOf OnFetchTimerTick
        End If
        FetchTimer.IsEnabled = False
        FetchTimer.Stop()
        SetSelectedItem(Nothing)
        If Editor.Text.Length > 0 Then
            IsLoading = True
            IsDropDownOpen = True
            ItemsSelector.ItemsSource = Nothing
            FetchTimer.IsEnabled = True
            FetchTimer.Start()
        Else
            IsDropDownOpen = False
        End If
    End Sub

    Private Sub OnFetchTimerTick(ByVal sender As Object, ByVal e As EventArgs)
        FetchTimer.IsEnabled = False
        FetchTimer.Stop()
        If Provider IsNot Nothing AndAlso ItemsSelector IsNot Nothing Then
            Filter = Editor.Text
            If _suggestionsAdapter Is Nothing Then
                _suggestionsAdapter = New SuggestionsAdapter(Me)
            End If
            _suggestionsAdapter.GetSuggestions(Filter)
        End If
    End Sub

    Private Sub OnPopupClosed(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub OnPopupOpened(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub OnSelectionAdapterCancel()
        IsDropDownOpen = False
    End Sub

    Private Sub OnSelectionAdapterCommit()
        SelectedItem = ItemsSelector.SelectedItem
        _isUpdatingText = True
        Editor.Text = GetDisplayText(ItemsSelector.SelectedItem)
        SetSelectedItem(ItemsSelector.SelectedItem)
        _isUpdatingText = False
        IsDropDownOpen = False
    End Sub

    Private Sub OnSelectionAdapterSelectionChanged()
        _isUpdatingText = True
        If ItemsSelector.SelectedItem Is Nothing Then
            Editor.Text = Filter
        Else
            Editor.Text = GetDisplayText(ItemsSelector.SelectedItem)
        End If
        Editor.SelectionStart = Editor.Text.Length
        Editor.SelectionLength = 0
        _isUpdatingText = False
    End Sub

    Private Sub SetSelectedItem(item As Object)
        _isUpdatingText = True
        SelectedItem = item
        _isUpdatingText = False
    End Sub
#End Region 'Methods

#Region "Nested Types"

    Private Class SuggestionsAdapter

#Region "Fields"

        Private _actb As AutoCompleteTextBox
        Private _filter As String

#End Region 'Fields

#Region "Constructors"

        Public Sub New(ByVal actb As AutoCompleteTextBox)
            _actb = actb
        End Sub

#End Region 'Constructors

#Region "Methods"

        Public Sub GetSuggestions(ByVal searchText As String)
            _filter = searchText
            _actb.IsLoading = True
            Dim thInfo As ParameterizedThreadStart = New ParameterizedThreadStart(AddressOf GetSuggestionsAsync)
            Dim th As New Thread(thInfo)
            th.Start(New Object() {searchText, _actb.Provider})
        End Sub

        Private Sub DisplaySuggestions(ByVal suggestions As IEnumerable, ByVal filter As String)
            If _filter <> filter Then
                Return
            End If
            If _actb.IsDropDownOpen Then
                _actb.IsLoading = False
                _actb.ItemsSelector.ItemsSource = suggestions
                _actb.IsDropDownOpen = _actb.ItemsSelector.HasItems
            End If
            
        End Sub

        Private Sub GetSuggestionsAsync(ByVal param As Object)
            Dim args As Object() = param
            Dim searchText As String = args(0)
            Dim provider As ISuggestionProvider = args(1)
            Dim list As IEnumerable = provider.GetSuggestions(searchText)
            _actb.Dispatcher.BeginInvoke(New Action(Of IEnumerable, String)(AddressOf DisplaySuggestions), DispatcherPriority.Background, New Object() {list, searchText})
        End Sub

#End Region 'Methods

    End Class

#End Region 'Nested Types

End Class