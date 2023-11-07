import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';
import Login from '@/pages/Login';
import ForgotPassword from '@/pages/ForgotPassword';
import ChangePassword from '@/pages/ChangePassword/index.js';

export const noLayoutRouteConfigs = [
  {
    path: '/register',
    component: Register,
  },
  {
    path: '/verify-email',
    component: VerifyEmail,
  },
  {
    path: '/login',
    component: Login,
  },
  {
    path: '/forgot-password',
    component: ForgotPassword,
  },
  {
    path: '/change-password',
    component: ChangePassword,
  },
];
