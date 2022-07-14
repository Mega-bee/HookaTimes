using HookaTimes.MVC.Areas._keenthemes.libs;

namespace HookaTimes.MVC.Areas._keenthemes;

public interface IKTBootstrapBase
{
    void initThemeMode();

    void initThemeDirection();

    void initLayout();

    void init(IKTTheme theme);
}