import classNames from 'classnames/bind';
import style from './Homepage.module.scss'
import { Input, Button } from 'antd';

const cx = classNames.bind(style);

function Homepage() {
  return <div className={cx("home-container w-full h-full m-auto bg-slate-500 block")}>
    <Input className={cx('search-input')} placeholder="Search parking zone" />
    <Button className={cx('btn')}>Find near parking zone</Button>
  </div>;
}

export default Homepage;
