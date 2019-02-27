import { Directive, ElementRef, Renderer2, OnInit, Input } from '@angular/core';

@Directive({
  selector: "[ngx-role]"
})
export class NgxRoleDirective implements OnInit {

  @Input("ngx-role") roles: Array<string>;

  constructor(
    private element: ElementRef,
    private renderer: Renderer2) {
  }

  ngOnInit(): void {
    const data: Array<string> = JSON.parse(localStorage.getItem("role"));
    const hasAuth = data.some(d => {
      return this.roles.some(r => {
        if (d === r) {
          return true;
        }
        return false;
      });
    });
    
    if (!hasAuth) {
      this.renderer.setStyle(this.element.nativeElement, "display", "node");
    }
  }
}
