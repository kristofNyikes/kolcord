import { RoundImageProps } from '../../types/types';

const RoundedImage: React.FC<RoundImageProps> = ({src, size}) => {
  const sizeList = {
    '8': 'size-8',
    '11': 'size-11',
    '32': 'size-32',
  }
  return (
    <div className='flex'>
      <div className='flex items-center'>
        <img src={src} alt="friend avatar" className={`inline-block ${sizeList[size]} rounded-full object-cover border-2 border-black/30`}/>
      </div>
    </div>
  );
};

export default RoundedImage;
