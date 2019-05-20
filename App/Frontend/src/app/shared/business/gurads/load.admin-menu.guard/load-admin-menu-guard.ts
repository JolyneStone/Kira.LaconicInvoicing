import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MenuService } from '@delon/theme';
import { adminMenu } from '@shared/business/admin-menu';

@Injectable({
  providedIn: 'root'
})
export class LoadAdminMenuGuard implements CanActivate  {

  constructor(
    private router: Router,
    private menuService: MenuService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    this.menuService.clear();
    this.menuService.add(adminMenu);
    return true;
  }
}
