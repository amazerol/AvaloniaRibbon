# Avalonia Ribbon
Ribbon Control for Avalonia

The purpose of this Avalonia component is to achieve something like Microsoft Ribbon UI found in Office and all around Windows operating system. The component can be used in its present state but is definitely not considered as complete. I had to implement ribbon interface for my project **[Jaya - Cross Plat](https://github.com/waliarubal/Jaya)** so had to tailor things accordingly. 

The original piece of work is by [Alban Mazerolles](https://github.com/amazerol) which can be found [here](https://github.com/amazerol/AvaloniaRibbon).

![Preview](https://i.imgur.com/r1LniW3.png)

Include ribbon styles to App.xaml as shown below.
```xaml
    <StyleInclude Source="avares://Avalonia.Controls.Ribbon/Styles/RibbonStyles.xaml" />
```

Use the below mentioned sample as an example to use the ribbon control. 
```xaml
    <RibbonWindow>
        <RibbonControl>
            <RibbonTab Header="RibbonTab 1">
                <StackPanel Orientation="Horizontal">
                    <RibbonTabGroup Text="Un premier groupe">
                        <StackPanel Orientation="Horizontal">
                            <RibbonButton IconPath="/Assets/RibbonIcons/settings.png" Text="Test3" />
                            <RibbonButton IconPath="/Assets/RibbonIcons/corner.png" Text="Test4" />
                            <RibbonButton IconPath="/Assets/RibbonIcons/chevron.png" Text="Test5" />
                            <RibbonButton IconPath="/Assets/RibbonIcons/settings.png" Text="Test6" />
                        </StackPanel>
                    </RibbonTabGroup>
                    <RibbonTabGroup Command="{Binding OnClickCommand}" Text="Paragraphe">
                        <StackPanel Orientation="Horizontal">
                            <StackPanel Orientation="Vertical">
                                <!--  Maximum 3 buttons per vertical stackpanel  -->
                                <RibbonLinearButton
                                    Command="{Binding OnClickCommand}"
                                    IconPath="/Assets/RibbonIcons/corner.png"
                                    Text="Reproduire la mise en forme" />
                                <RibbonLinearButton IconPath="/Assets/RibbonIcons/settings.png" Text="Test8" />
                                <RibbonLinearButton IconPath="/Assets/RibbonIcons/settings.png" Text="Test9" />
                            </StackPanel>
                            <RibbonComboButton IconPath="/Assets/RibbonIcons/settings.png" Text="Coller">
                                <ComboBoxItem>ksdlfml</ComboBoxItem>
                                <ComboBoxItem>ksdnvbl</ComboBoxItem>
                            </RibbonComboButton>
                            <StackPanel>
                                <RibbonSmallButtonHGroup>
                                    <RibbonSmallButton IconPath="/Assets/RibbonIcons/settings.png" ToolTip.Tip="A small tooltip" />
                                    <RibbonSmallButton IconPath="/Assets/RibbonIcons/settings.png" />
                                    <RibbonSmallButton IconPath="/Assets/RibbonIcons/settings.png" />
                                </RibbonSmallButtonHGroup>
                            </StackPanel>
                        </StackPanel>
                    </RibbonTabGroup>
                </StackPanel>
            </RibbonTab>
            <RibbonTab Header="RibbonTab 2">qsdfqsdf</RibbonTab>
        </RibbonControl>
    </RibbonWindow>
```

## Change Log

### Update (14/11/2019)
- Added separate sample project to demonstrate the usage.
- Architectural improvements have been done.
- Standardized control to be an assembly instead of executable.
- Added ribbon classes to Avalonia's namespace.

### Update (23/06/2019)
- In `App.xaml`, only one line of `<StyleInclude>` is required now.
- Adddition of the special buttons (top part: button, lower part: combobox)
- The entire control is themable now.
- Small button added and its HorizontalGroup.
- Tootips added.

### Update (17/06/2019)
- Control is now fully usable.
- NuGet done.
- Wiki has been written.

Below mentioned are some plans for the future.
- Implement more controls, especially toggle button.
- Allow color stylesheets for a fully customizable control.
- Take care of the resizing matters.

### Update (16/06/2019)
- Improvement of the look &amp; feel of the ribbon.
- Update of the previous preview image with new look &amp; feel.

The remaining things which have be done are as follows.
- Take care of the disapearance of icons in case of window resizing.
- Handle click actions easily.
