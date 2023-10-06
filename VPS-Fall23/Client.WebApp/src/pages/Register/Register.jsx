import classNames from 'classnames/bind';
import { useEffect } from 'react';

import * as authServices from '@/services/authService';
import styles from './Register.module.scss';

const cx = classNames.bind(styles);

function Register() {
  useEffect(() => {
    const fetchApi = async () => {
      await authServices.register();
    };

    fetchApi();
  }, []);
  return (
    <div className={cx('wrapper')}>
      <h1>Register Page</h1>
    </div>
  );
}

export default Register;
