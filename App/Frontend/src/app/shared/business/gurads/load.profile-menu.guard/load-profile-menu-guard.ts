import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { MenuService } from '@delon/theme';
import { profileMenu } from '@shared/business/profile-menu.';

@Injectable({
  providedIn: 'root'
})
export class LoadProfileMenuGuard implements CanActivate {
  constructor(
    private router: Router,
    private menuService: MenuService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    this.menuService.clear();
    this.menuService.add(profileMenu);
    return true;
  }
}
