import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { defaultMenu } from '@shared/business/default-menu';
import { MenuService } from '@delon/theme';

@Injectable({
  providedIn: 'root'
})
export class LoadDefaultMenuGuard implements CanActivate  {
  constructor(
    private router: Router,
    private menuService: MenuService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    this.menuService.clear();
    this.menuService.add(defaultMenu);
    return true;
  }
}
