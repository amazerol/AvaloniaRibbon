# Avalonia Ribbon
Ribbon Control for Avalonia

The purpose of this Avalonia component is to achieve something like Microsoft Ribbon UI found in Office and all around Windows operating system. The component can already be used but is definitely not considered as complete. I had to implement ribbon interface for my project **[Jaya - Cross Plat](https://github.com/waliarubal/Jaya)** so had to tailor things accordingly. 

The original piece of work is from [Alban Mazerolles](https://github.com/amazerol) which can be found [here](https://github.com/amazerol/AvaloniaRibbon).

![Preview](https://user-images.githubusercontent.com/16206389/59566444-a752ea80-9060-11e9-828b-593d347f797d.gif)

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



