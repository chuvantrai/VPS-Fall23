import Header from '@/layouts/components/Header';
import AccountProfile from '@/pages/AccountProfile/';
import ViewListParkingZone from '@/pages/Homepage/components/Content/ViewListParkingZone';
import RegisterParkingZone from '@/pages/RegisterParkingZone';
import ListAttendant from '@/pages/ListAttendant';
import ManagerLayout from '../layouts/ManagerLayout';
import guidGenerator from '../helpers/guidGenerator';
import ListFeedback from '../pages/ListFeedback/ListFeedback';
import Overview from '../pages/Dashboard/Overview';
import PromoCode from '../pages/PromoCode/PromoCode';

export const ownerRoutesConfig = {
  header: Header,
  footer: null,
  layout: ManagerLayout,
  routes: [
    {
      key: guidGenerator(),
      path: '',
      label: 'Trang chủ',
      component: Overview,
      description: '',
    },
    {
      key: guidGenerator(),
      path: 'user',
      label: 'Người dùng',
      children: [
        {
          key: guidGenerator(),
          path: 'profile',
          label: 'Thông tin cá nhân',
          component: AccountProfile,
          description: 'Thông tin cá nhân tài khoản đăng nhập',
        },
      ],
    },
    {
      key: guidGenerator(),
      path: 'parking-zone',
      label: 'Bãi đỗ xe',
      children: [
        {
          key: guidGenerator(),
          path: 'list-parking-zone',
          label: 'Danh sách bãi đỗ xe',
          component: ViewListParkingZone,
          description: 'Toàn bộ danh sách bãi gửi xe hiển thị dưới đây',
          // children: [
          //   {
          //     key: guidGenerator(),
          //     path: 'overview',
          //     label: 'Overview',
          //     component: BookedOverview,
          //     description: 'Overview',
          //   },
          // ],
        },
        {
          key: guidGenerator(),
          path: 'register-new-parking-zone',
          label: 'Đăng ký bãi đỗ xe',
          component: RegisterParkingZone,
          description: 'Điền vào mẫu dưới đây để đăng ký bãi gửi xe mới',
        },
      ],
    },
    {
      key: guidGenerator(),
      path: 'attendant',
      label: 'Nhân viên',
      children: [
        {
          key: guidGenerator(),
          path: 'list-attendant',
          label: 'Danh sách nhân viên',
          component: ListAttendant,
          description: 'Toàn bộ danh sách nhân viên hiển thị dưới đây',
        },
      ],
    },
    {
      key: guidGenerator(),
      path: 'utilities',
      label: 'Tiện ích',
      children: [
        {
          key: guidGenerator(),
          path: 'feedback',
          label: 'Feedback',
          component: ListFeedback,
          description: 'Danh sách phản hồi từ người dùng',
        },
        {
          key: guidGenerator(),
          path: 'promo-code',
          label: 'Mã khuyến mãi',
          component: PromoCode,
          description: 'Danh sách mã khuyến mãi',
        },
      ],
    },
  ],
};
