import { driverRoutesConfig } from './driver.routes';
import { adminRoutesConfig } from './admin.routes';
import { ownerRoutesConfig } from './owner.routes';

export const mainLayoutRoutesConfig = {
  0: driverRoutesConfig,
  1: adminRoutesConfig,
  2: ownerRoutesConfig,
};
